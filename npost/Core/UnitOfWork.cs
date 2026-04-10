using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using npost.Data;
using npost.Middlewares;

namespace npost.Core;

public class UnitOfWork
{

    private IDbContextTransaction _transaction;
    private readonly DataContext _context;

    public UnitOfWork(DataContext context)
    {
        _context = context;
        if (_context.Database.CurrentTransaction == null)
        {
            _transaction = _context.Database.BeginTransaction();
        }
        else
        {
            _transaction = _context.Database.CurrentTransaction;
            _context.Database.UseTransaction(_transaction.GetDbTransaction());
        }
    }

    public async Task<int> SaveAsync()
    {
        try
        {
            var RowsAffected = await _context.SaveChangesAsync();
            if (RowsAffected == 0)
            {
                await _transaction.RollbackAsync();
                throw new BusinessException("Nenhum registro foi afetado.");
            }

            return RowsAffected;
        }
        catch (Exception ex)
        {
            var msg = GetExceptionMessages(ex);
            await _transaction.RollbackAsync();
            throw new BusinessException(msg);
        }
    }

    public async Task CommitAsync()
    {
        try
        {
            await _transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            var msg = GetExceptionMessages(ex);
            await _transaction.RollbackAsync();
            throw new BusinessException(msg);
        }
    }

    private string GetExceptionMessages(Exception ex)
    {
        string msg;

        //Para mais detalhes:
        //id = ex.InnerException.HResult;
        //detail = ex.InnerException.Message;
        if (ex.InnerException != null && ex.InnerException.HResult == -2146232060)
        {
            //"A instrução INSERT conflitou com a restrição do FOREIGN KEY \"fkCursosGrausInstrucoes\".
            //O conflito ocorreu no banco de dados \"mdi\", tabela \"dbo.GrausInstrucoes\", column 'grausInstrucoesId'."
            msg = "Erro em registro vinculado.";

        }
        else if (ex.InnerException != null && ex.InnerException is PostgresException && ex.InnerException.HResult == -2147467259)
        {
            //23503: atualização ou exclusão em tabela "GrupoUsuarios" viola restrição de chave estrangeira "fkPermissoesGruposUsuarios" em "Permissoes"
            //DETAIL: Detail redacted as it may contain sensitive data. Specify 'Include Error Detail' in the connection string to include this information.

            var constraint = (ex.InnerException as PostgresException)!.ConstraintName ?? "";
            msg = Constraint.Description(constraint);
        }

        else if (ex.HResult == -2146233088)
        {
            //Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException: 
            //'The database operation was expected to affect 1 row(s), but
            //actually affected 0 row(s); data may have been modified or
            //deleted since entities were loaded. See
            //http://go.microsoft.com/fwlink/?LinkId=527962 for information
            //on understanding and handling optimistic concurrency exceptions.'
            msg = "Registro não encontrado, nenhum registro foi afetado.";
        }
        else
        {
            msg = "Erro não tratado, nenhum registro foi afetado.";
        }

        return msg;
    }
}