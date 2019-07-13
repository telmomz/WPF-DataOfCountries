namespace WPFUI.Services
{
    ///References
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Windows;
    using WPFUI.Models;

    /// <summary>
    /// Data service
    /// </summary>
    public class DataService
    {
        /// <summary>
        /// SQLite version 3 Initializer
        /// </summary>
        public DataService()
        {
            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");
                try
                {
                    CreateDB();
                }
                catch (Exception e)
                {
                    Console.WriteLine("/DataService/" + Environment.NewLine + e.Message);
                    MessageBox.Show(e.Message, "Data Service");
                }
            }

        }

        #region Set DB Methods

        /// <summary>
        /// Sets data on DB
        /// </summary>
        /// <param name="_Countries">Collection of Countries</param>
        public void setCountryData(List<Country> Countries)
        {
            DataTable dataTable = new DataTable();

            string sql  = "select * from Country";
            dataTable   = DataAccess.GetTable(sql);
            WriteCountryOnDB(Countries);
            
        }

        /// <summary>
        /// Sets data on DB
        /// </summary>
        /// <param name="_Countries">Collection of Rates</param>
        public void setRateData(List<Rate> Rates)
        {
            DataTable dataTable = new DataTable();

            string sql = "select * from Rates";
            dataTable = DataAccess.GetTable(sql);
            WriteRateOnDB(Rates);
        }

        /// <summary>
        /// Creates the tables and file
        /// </summary>
        private void CreateDB()
        {
            TableCountry();
            TableRates();
        }
        
        /// <summary>
        /// Writes values to local DB
        /// </summary>
        /// <param name="table">DataTable</param>
        /// <param name="Countries">Collection of Countries</param>
        private void WriteCountryOnDB(List<Country> Countries)
        {
            int i = 0;
            try
            {
                foreach (var country in Countries)
                {
                    string sql = string.Format("INSERT INTO Country(" +
"`Name`, `TopLevelDomain0`, `TopLevelDomain1`, `TopLevelDomain2`, `Alpha2Code`, `Alpha3Code`, `CallingCodes0`, `CallingCodes1`, `CallingCodes2`, " +
"`Capital`, `Region`, `Subregion`, `Population`, `LatLng0`, `LatLng1`, `NativeName`, `NumericCode`, `Currency_code`, `Currency_name`, `Currency_symbol`, `Gini`, `Flag`, " +
"`RB_Acronym`, `RB_Name`, `RB_OtherAcronym0`, `RB_OtherAcronym1`, `RB_OtherAcronym2`, `RB_OtherName0`, `RB_OtherName1`, `RB_OtherName2`, `RB_OtherName3`, `RB_OtherName4`) " +
"VALUES(" +
"@Name, @TopLevelDomain0, @TopLevelDomain1, @TopLevelDomain2, @Alpha2Code, @Alpha3Code, @CallingCodes0, @CallingCodes1, @CallingCodes2, @Capital, @Region, @Subregion, @Population, " +
"@LatLng0, @LatLng1, @NativeName, @NumericCode, @Currency_code, @Currency_name ,@Currency_symbol, @Gini, @Flag, @RB_Acronym, @RB_Name, " +
"@RB_OtherAcronym0, @RB_OtherAcronym1, @RB_OtherAcronym2, @RB_OtherName0, @RB_OtherName1, @RB_OtherName2, @RB_OtherName3, @RB_OtherName4)");

                    DataAccess.StoreCountry(sql, country);
                    i++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("/GetDataTableCountry/" + Environment.NewLine + e.Message);
                MessageBox.Show(e.Message, "GetDataTableCountry");
            }
        }

        /// <summary>
        /// Writes values to local DB
        /// </summary>
        /// <param name="table">DataTable</param>
        /// <param name="Rates">Collection of Rates</param>
        private void WriteRateOnDB(List<Rate> Rates)
        {
            try
            {
                foreach (var rate in Rates)
                {
                    string sql = string.Format(
                        "INSERT INTO Rates (RateId, Code, TaxRate, Name) " +
                        "values(@RateId, @Code, @TaxRate, @Name)"
                        );

                    DataAccess.StoreRates(sql, rate);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("/GetDataTableRate/" + Environment.NewLine + e.Message);
                MessageBox.Show(e.Message, "GetDataTableRate");
            }
        }

        /// <summary>
        /// Creates table on local db named Rates
        /// </summary>
        private void TableRates()
        {
            string sql = "CREATE TABLE Rates(RateId int, Code varchar(5), TaxRate real, Name varchar(250))";

            DataAccess.GetDataSet(sql);
        }

        /// <summary>
        /// Creates table on local db named Country
        /// </summary>
        private void TableCountry()
        {
            string sql = "CREATE TABLE Country(" +
"`IdCountry` INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE, `Name` nvarchar(120), `TopLevelDomain0` nvarchar(120), `TopLevelDomain1` nvarchar(120), `TopLevelDomain2` nvarchar(120), `Alpha2Code` nvarchar(10), `Alpha3Code` nvarchar(10), `CallingCodes0` nvarchar(120), " +
"`CallingCodes1` nvarchar(120), `CallingCodes2` nvarchar(120), `Capital` nvarchar(120), `Region` nvarchar(200), `Subregion` nvarchar(100), `Population` int, `LatLng0` nvarchar(50), `LatLng1` nvarchar(50), `NativeName` nvarchar(120), " +
"`NumericCode` nvarchar(50),`Currency_code` nvarchar(50), `Currency_name` nvarchar(50), `Currency_symbol` nvarchar(50), `Gini` decimal(18, 6), `Flag` nvarchar(250), `RB_Acronym` nvarchar(250), `RB_Name` nvarchar(250), `RB_OtherAcronym0` nvarchar(250), `RB_OtherAcronym1` nvarchar(250), `RB_OtherAcronym2` nvarchar(250), `RB_OtherName0` nvarchar(250), `RB_OtherName1` nvarchar(250)," +
"`RB_OtherName2` nvarchar(250), `RB_OtherName3` nvarchar(250), `RB_OtherName4` nvarchar(250))";

            DataAccess.GetDataSet(sql);
        }
        
        #endregion

    }

}
