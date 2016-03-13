using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using System.Threading.Tasks;
using net.sf.jsqlparser;
using net.sf.jsqlparser.expression;
using net.sf.jsqlparser.parser;
using net.sf.jsqlparser.statement;
using net.sf.jsqlparser.util;
using net.sf.jsqlparser.statement.select;
using net.sf.jsqlparser.statement.insert;
using net.sf.jsqlparser.statement.update;
using net.sf.jsqlparser.statement.delete;


namespace TestSql
{
    class Program
    {
        static void Main(string[] args)
        {
            getTablesBySqlParser();
            getSqlTablesByRegEx();
            

            Console.ReadLine();
        }

        private static void getTablesBySqlParser()
        {
            string sqls = "select * from [dbo].[TSEL1], [dbo].[TSEL2]  where [TSEL1].rownum <= @hello";
            sqls = "select * from [dbo].[TSEL1], [dbo].[TSEL2]  where [TSEL1].rownum <= @hello";
            sqls = "select * from [dbo].[TSEL1], [dbo].[TSEL2]  where [TSEL1].rownum <= @hello";
            string sqlu = "UPDATE TUPD set BB = '123'";
            string sqld = "DELETE FROM TDEL where BB in (select dd from aaa where cc='123');";
            string sqli = "insert into czyb(yhm,mm,qx) values (@yhm,@mm,@qx)";

            sqls = Regex.Replace(Regex.Replace(sqls,@"@\w+","123"), @"[\[\]]", "").ToUpper();
            sqli = Regex.Replace(sqli,@"@\w+","123").ToUpper();

            //try translate to Select 
            Statement statement = CCJSqlParserUtil.parse(sqls);
            Select selectStatement = (Select)statement;
            TablesNamesFinder tablesNamesFinder = new TablesNamesFinder();
            object[] tableList = tablesNamesFinder.getTableList(selectStatement).toArray();
            printTables(tableList,"SELECT");

            //try translate to Update 
            statement = CCJSqlParserUtil.parse(sqlu);
            Update updateStatement = (Update)statement;
            tablesNamesFinder = new TablesNamesFinder();
            tableList = tablesNamesFinder.getTableList(updateStatement).toArray();
            printTables(tableList, "UPDATE");

            //try translate to Delete 
            statement = CCJSqlParserUtil.parse(sqld);
            Delete deleteStatement = (Delete)statement;
            Expression ep = deleteStatement.getWhere();


            tablesNamesFinder = new TablesNamesFinder();
            tableList = tablesNamesFinder.getTableList(deleteStatement).toArray();
            printTables(tableList, "DELETE");

            //try translate to Insert 
            //try translate to Insert 
            //try translate to Insert 
            //try translate to Insert 
            //try translate to Insert 
            statement = CCJSqlParserUtil.parse(sqli);
            Insert insertStatement = (Insert)statement;
            tablesNamesFinder = new TablesNamesFinder();
            tableList = tablesNamesFinder.getTableList(insertStatement).toArray();
            printTables(tableList, "INSERT");
        }

        private static List<string> printTables(object[] tableList,string prefix)
        {
            List<string> result = new List<string>();
            if (tableList == null || tableList.Length == 0) {
                result.Add(prefix + "\t" + "N/A");
                return result;

            }
            for (int i = 0; i < tableList.Length; i++)
            {
                if (i != 0 && "SELECT".ToUpper()!= prefix)
                {
                    result.Add(prefix + " WHERE SELECT" + "\t" + tableList[i]);
                }
                else
                {
                    result.Add(prefix + "\t" + tableList[i]);
                }
            }
            foreach (var item in result)
            {
                string tt = item as string;
                Console.WriteLine(tt);
            }

            return result;
        }


        private static void getSqlTablesByRegEx()
        {
            string[] arrs = new string[] {
                "SELECT * FROM Config",
"SELECT * FROM [dbo].IMEIUser",
"SELECT * FROM dbo.LotteryLog",
"SELECT * FROM [GreenPrize]",
"SELECT * FROM [dbo].[Config]",
"SELECT * FROM dbo.[Prize]",
"SELECT * FROM [DBName].[dbo].[Config]}" };



            string rex = @".*\s+FROM\s+[\w\[\]]*\.?[\w\[\]]*\.?\[?(\b\w+)\]?[\r\n\s]*";

            if (Regex.IsMatch(arrs[1].ToUpper(), rex)) {
                Match mat = Regex.Match(arrs[1], rex);
                Console.WriteLine(mat.Result("$1"));
            }

            Console.ReadLine();
        }
    }
}
