/**

CREATE TABLE [dbo].[TargetTable](
  [Value] [nvarchar](max) NULL
)

 **/
[TestFixture]
public class Tests
{
    [Test]
    public void Foo()
    {
#if NET8_0
        // required to run. but does note escalate. stop MSDTC service to ensure
        TransactionManager.ImplicitDistributedTransactions = true;
#endif
        using var scope = new TransactionScope();

        using var connection1 = new SqlConnection("Data Source=.;Database=Db1; Integrated Security=True;Max Pool Size=100;Encrypt=False");
        connection1.Open();
        using var command1 = connection1.CreateCommand();
        command1.CommandText = "INSERT INTO TargetTable (Value) VALUES ('content')";
        command1.ExecuteNonQuery();

        using var connection2 = new SqlConnection("Data Source=.;Database=Db2; Integrated Security=True;Max Pool Size=100;Encrypt=False");
        connection2.Open();
        using var command2 = connection2.CreateCommand();
        command2.CommandText = "INSERT INTO TargetTable (Value) VALUES ('content')";
        command2.ExecuteNonQuery();

        scope.Complete();
    }
}