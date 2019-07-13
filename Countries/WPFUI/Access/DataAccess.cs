namespace WPFUI.Services
{
    ///References
    using Newtonsoft.Json.Linq;
    using Svg;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SQLite;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Media.Imaging;
    using WPFUI.Models;

    /// <summary>
    /// System.Object DataAccess 
    /// </summary>
    public class DataAccess
    {
        #region SQLite Access Atributes

        static string _conDB = @"Data\CountryRegistry.sqlite;Version=3;New=False;Compress=True";
        
        static SQLiteConnection _con = null;

        #endregion

        #region SQLite Access Propreties

        /// <summary>
        /// Connection for local DB
        /// </summary>
        public static SQLiteConnection Connection
        {
            get
            {
                if (_con == null)
                {
                    _con = new SQLiteConnection("Data Source=" + _conDB);
                    _con.Open();
                    return _con;

                }

                else if (_con.State != ConnectionState.Open)
                {
                    _con.Open();
                    return _con;
                }

                else
                {
                    return _con;
                }
            }
        }

        #endregion

        #region SQLite Access Methods

        /// <summary>
        /// Get requested sql query
        /// </summary>
        /// <param name="sql">SQL query</param>
        /// <returns>System.Data</returns>
        public static DataSet GetDataSet(string sql)
        {
            Console.WriteLine(sql);
            SQLiteCommand cmd = new SQLiteCommand(sql, Connection);
            SQLiteDataAdapter adp = new SQLiteDataAdapter(cmd);

            DataSet ds = new DataSet();
            adp.Fill(ds);
            Connection.Close();

            return ds;
        }

        /// <summary>
        /// Gets table requested sql query
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>System.DataTale</returns>
        public static DataTable GetTable(string sql)
        {
            DataSet ds = GetDataSet(sql);

            try
            {
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine("/GetDataTable/" + Environment.NewLine + e.Message);
                return null;
            }
        }

        /// <summary>
        /// Backup db
        /// </summary>
        public static void BackupDB()
        {
            string date = $"{DateTime.Now.Day}-{DateTime.Now.Month}";
            string sql = $"DATA SOURCE = " + 
                $@"Data\CountryRegistry.{date}.sqlite;Version=3;New=False;Compress=True";

            using (var bak = new SQLiteConnection(sql))
            {
                bak.Open();
                Connection.BackupDatabase(bak, "main", "main", -1, null, 0);
            }

            Connection.Close();
        }

        /// <summary>
        /// Drops values on db 
        /// </summary>
        public static void DropDB()
        {
            string sql;

            sql = "DELETE FROM Country";
            SQLiteCommand cmd = new SQLiteCommand(sql, Connection);
            SQLiteDataAdapter adp = new SQLiteDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sql = "VACUUM";
            cmd = new SQLiteCommand(sql, Connection);
            adp = new SQLiteDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sql = "DELETE FROM Rates";
            cmd = new SQLiteCommand(sql, Connection);
            adp = new SQLiteDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sql = "VACUUM";
            cmd = new SQLiteCommand(sql, Connection);
            adp = new SQLiteDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            Connection.Close();
        }

        /// <summary>
        /// Gets local data of countries
        /// </summary>
        /// <returns>Collection of countries</returns>
        public static List<Country> StoredCountry()
        {
            List<Country> Countries = new List<Country>();
            DataSet ds              = new DataSet();


            string date = $"{DateTime.Now.Day}-{DateTime.Now.Month}";
            string con  = $"DATA SOURCE = " +
                $@"Data\CountryRegistry.{date}.sqlite;Version=3;New=False;Compress=True";
            string sql  = "SELECT * FROM Country";


            using (var bak = new SQLiteConnection(con))
            {
                Console.WriteLine(sql);
                SQLiteCommand cmd       = new SQLiteCommand(sql, bak);
                SQLiteDataAdapter adp   = new SQLiteDataAdapter(cmd);

                adp.Fill(ds);

                try
                {
                    if (ds.Tables.Count > 0)
                    {
                        DataTable temp = ds.Tables[0];
                        Countries = temp
                                .AsEnumerable()
                                .Select(x => new Country()
                                {
                                    name            = x.Field<string>("Name"),
                                    topLevelDomain  = new List<string>
                                    {
                                        x.Field<string>("TopLevelDomain0"),
                                        x.Field<string>("TopLevelDomain1"),
                                        x.Field<string>("TopLevelDomain2")
                                    },
                                    alpha2Code      = x.Field<string>("Alpha2Code"),
                                    alpha3Code      = x.Field<string>("Alpha3Code"),
                                    callingCodes    = new List<string>
                                    {
                                        x.Field<string>("CallingCodes0"),
                                        x.Field<string>("CallingCodes1"),
                                        x.Field<string>("CallingCodes2")
                                    },
                                    capital         = x.Field<string>("Capital"),
                                    region          = x.Field<string>("Region"),
                                    subregion       = x.Field<string>("Subregion"),
                                    population      = x.Field<int>("Population"),
                                    latlng          = new List<object>
                                    {
                                        x.Field<string>("LatLng0"),
                                        x.Field<string>("LatLng1")
                                    },
                                    nativeName      = x.Field<string>("NativeName"),
                                    numericCode     = x.Field<string>("NumericCode"),
                                    currencies      = new List<Currency>
                                    {
                                        new Currency()
                                        {
                                            code    = x.Field<string>("Currency_code"),
                                            name    = x.Field<string>("Currency_name"),
                                            symbol  = x.Field<string>("Currency_symbol")
                                        }
                                    },
                                    gini            = Convert.ToDouble(x.Field<decimal>("Gini")),
                                    regionalBlocs   = new List<object>
                                    {
                                        x.Field<string>("RB_Acronym"),
                                        x.Field<string>("RB_Name"),
                                        x.Field<string>("RB_OtherAcronym0"),
                                        x.Field<string>("RB_OtherAcronym1"),
                                        x.Field<string>("RB_OtherName0"),
                                        x.Field<string>("RB_OtherName1"),
                                        x.Field<string>("RB_OtherName2"),
                                        x.Field<string>("RB_OtherName3"),
                                        x.Field<string>("RB_OtherName4")
                                    }
                                })
                                .ToList();

                            return Countries;
                    }
                    return Countries;
                }
                catch (Exception e)
                {
                    Console.WriteLine("/GetDataTable/" + Environment.NewLine + e.Message);

                    return Countries;
                }
            }
        }

        /// <summary>
        /// Gets local data of rates
        /// </summary>
        /// <returns>Collection of rates</returns>
        public static List<Rate> StoredRates()
        {
            List<Rate> Rates = new List<Rate>();
            DataSet ds = new DataSet();


            string date = $"{DateTime.Now.Day}-{DateTime.Now.Month}";
            string con = $"DATA SOURCE = " +
                $@"Data\CountryRegistry.{date}.sqlite;Version=3;New=False;Compress=True";
            string sql = "SELECT * FROM Rates";


            using (var bak = new SQLiteConnection(con))
            {
                Console.WriteLine(sql);
                SQLiteCommand cmd       = new SQLiteCommand(sql, bak);
                SQLiteDataAdapter adp   = new SQLiteDataAdapter(cmd);

                adp.Fill(ds);
                try
                {
                    if (ds.Tables.Count > 0)
                    {
                        DataTable temp = ds.Tables[0];
                        Rates = temp
                            .AsEnumerable()
                            .Select(x => new Rate()
                            {
                                RateId  = x.Field<int>("RateId"),
                                Code    = x.Field<string>("Code"),
                                TaxRate = x.Field<double>("TaxRate"),
                                Name    = x.Field<string>("Name")
                            })
                            .ToList();
                        return Rates;
                    }
                    return Rates;
                }
                catch (Exception e)
                {
                    Console.WriteLine("/GetDataTableRate/" + Environment.NewLine + e.Message);

                    return Rates;
                }
            }
        }

        /// <summary>
        /// Writes on DB the requested sql query
        /// </summary>
        /// <param name="sql">SQL query</param>
        /// <param name="country">Class Country</param>
        public static void StoreCountry(string sql, Country country)
        {
            SQLiteCommand cmd = new SQLiteCommand(sql, Connection);
            
            try
            {
                cmd.Parameters.AddWithValue("@Name", country.name);
            }
            catch
            {
                cmd.Parameters.AddWithValue("@Name", "none");
            }
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    cmd.Parameters.AddWithValue($"@TopLevelDomain{i}", country.topLevelDomain[i]);
                }
                catch
                {
                    cmd.Parameters.AddWithValue($"@TopLevelDomain{i}", "none");
                }

            }
            try
            {
                cmd.Parameters.AddWithValue("@Alpha2Code", country.alpha2Code);
            }
            catch
            {
                cmd.Parameters.AddWithValue("@Alpha2Code", "none");
            }
            try
            {
                cmd.Parameters.AddWithValue("@Alpha3Code", country.alpha3Code);
            }
            catch
            {
                cmd.Parameters.AddWithValue("@Alpha3Code", "none");
            }
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    if (!string.IsNullOrEmpty(country.callingCodes[i]))
                        cmd.Parameters.AddWithValue($"@CallingCodes{i}", country.callingCodes[i]);
                    else
                        cmd.Parameters.AddWithValue($"@CallingCodes{i}", "none");
                }
                catch
                {
                    cmd.Parameters.AddWithValue($"@CallingCodes{i}", "none");
                }
            }
            try
            {
                if (!string.IsNullOrEmpty(country.capital))
                    cmd.Parameters.AddWithValue("@Capital", country.capital);
                else
                    cmd.Parameters.AddWithValue("@Capital", "none");
            }
            catch
            {
                cmd.Parameters.AddWithValue("@Capital", "none");
            }
            try
            {
                if (!string.IsNullOrEmpty(country.region))
                    cmd.Parameters.AddWithValue("@Region", country.region);
                else
                    cmd.Parameters.AddWithValue("@Region", "none");
            }
            catch
            {
                cmd.Parameters.AddWithValue("@Region", "none");
            }
            try
            {
                if (!string.IsNullOrEmpty(country.region))
                    cmd.Parameters.AddWithValue("@Subregion", country.subregion);
                else
                    cmd.Parameters.AddWithValue("@Subregion", "none");
            }
            catch
            {
                cmd.Parameters.AddWithValue("@Subregion", "none");
            }
            try
            {
                cmd.Parameters.AddWithValue("@Population", country.population);
            }
            catch
            {
                cmd.Parameters.AddWithValue("@Population", 000000);
            }
            for (int i = 0; i < 2; i++)
            {
                try
                {
                    cmd.Parameters.AddWithValue($"@LatLng{i}", country.latlng[i]);
                }
                catch
                {
                    cmd.Parameters.AddWithValue($"@LatLng{i}", 33.00);
                }
            }
            try
            {
                if (!string.IsNullOrEmpty(country.nativeName))
                    cmd.Parameters.AddWithValue("@NativeName", country.nativeName);
                else
                    cmd.Parameters.AddWithValue("@NativeName", "none");
            }
            catch
            {
                cmd.Parameters.AddWithValue("@NativeName", "none");
            }
            try
            {
                if (!string.IsNullOrEmpty(country.numericCode))
                    cmd.Parameters.AddWithValue("@NumericCode", country.numericCode);
                else
                    cmd.Parameters.AddWithValue("@NumericCode", "none");
            }
            catch
            {
                cmd.Parameters.AddWithValue("@NumericCode", "none");
            }
            try
            {
                foreach (var item in country.currencies)
                {
                    if (!string.IsNullOrEmpty(item.code))
                        cmd.Parameters.AddWithValue("@Currency_code", item.code);
                    else
                        cmd.Parameters.AddWithValue("@Currency_code", "none");

                    if (!string.IsNullOrEmpty(item.name))
                        cmd.Parameters.AddWithValue("@Currency_name", item.name);
                    else
                        cmd.Parameters.AddWithValue("@Currency_name", "none");

                    if (!string.IsNullOrEmpty(item.symbol))
                        cmd.Parameters.AddWithValue("@Currency_symbol", item.symbol);
                    else
                        cmd.Parameters.AddWithValue("@Currency_symbol", "none");
                }

            }
            catch
            {
                cmd.Parameters.AddWithValue("@Currency_code", "none");
                cmd.Parameters.AddWithValue("@Currency_name", "none");
                cmd.Parameters.AddWithValue("@Currency_symbol", "none");
            }
            try
            {
                if (country.gini != null)
                    cmd.Parameters.AddWithValue("@Gini", country.gini);
                else
                    cmd.Parameters.AddWithValue("@Gini", 100.00);
            }
            catch
            {
                cmd.Parameters.AddWithValue("@Gini", 100.00);
            }
            try
            {
                cmd.Parameters.AddWithValue("@Flag", country.flag);
            }
            catch
            {
                cmd.Parameters.AddWithValue("@Flag", "none");
            }
            for (int i = 0; i < 1; i++)
            {
                dynamic tempL = JArray.FromObject(country.regionalBlocs);

                try
                {
                    dynamic temp = tempL[0];
                    
                    try
                    {
                        cmd.Parameters.AddWithValue("@RB_Acronym", temp.acronym);
                    }
                    catch
                    {
                        cmd.Parameters.AddWithValue("@RB_Acronym", "none");
                    }
                    try
                    {
                        cmd.Parameters.AddWithValue("@RB_Name", temp.name);
                        cmd.Parameters.AddWithValue($"@RB_OtherAcronym0", "none");
                        cmd.Parameters.AddWithValue($"@RB_OtherAcronym1", "none");
                        cmd.Parameters.AddWithValue($"@RB_OtherAcronym2", "none");
                        cmd.Parameters.AddWithValue($"@RB_OtherName0", "none");
                        cmd.Parameters.AddWithValue($"@RB_OtherName1", "none");
                        cmd.Parameters.AddWithValue($"@RB_OtherName2", "none");
                        cmd.Parameters.AddWithValue($"@RB_OtherName3", "none");
                        cmd.Parameters.AddWithValue($"@RB_OtherName4", "none");
                    }
                    catch
                    {
                        cmd.Parameters.AddWithValue("@RB_Name", "none");
                        cmd.Parameters.AddWithValue($"@RB_OtherAcronym0", "none");
                        cmd.Parameters.AddWithValue($"@RB_OtherAcronym1", "none");
                        cmd.Parameters.AddWithValue($"@RB_OtherAcronym2", "none");
                        cmd.Parameters.AddWithValue($"@RB_OtherName0", "none");
                        cmd.Parameters.AddWithValue($"@RB_OtherName1", "none");
                        cmd.Parameters.AddWithValue($"@RB_OtherName2", "none");
                        cmd.Parameters.AddWithValue($"@RB_OtherName3", "none");
                        cmd.Parameters.AddWithValue($"@RB_OtherName4", "none");
                    }
                }
                catch
                {
                    cmd.Parameters.AddWithValue("@RB_Acronym", "none");
                    cmd.Parameters.AddWithValue("@RB_Name", "none");
                    cmd.Parameters.AddWithValue($"@RB_OtherAcronym0", "none");
                    cmd.Parameters.AddWithValue($"@RB_OtherAcronym1", "none");
                    cmd.Parameters.AddWithValue($"@RB_OtherAcronym2", "none");
                    cmd.Parameters.AddWithValue($"@RB_OtherName0", "none");
                    cmd.Parameters.AddWithValue($"@RB_OtherName1", "none");
                    cmd.Parameters.AddWithValue($"@RB_OtherName2", "none");
                    cmd.Parameters.AddWithValue($"@RB_OtherName3", "none");
                    cmd.Parameters.AddWithValue($"@RB_OtherName4", "none");
                }

            }

            cmd.ExecuteNonQuery();
            Connection.Close();
        }


        /// <summary>
        /// Writes on DB the requested sql query
        /// </summary>
        /// <param name="sql">SQL query</param>
        /// <param name="country">Class Rate</param>
        public static void StoreRates(string sql, Rate rates)
        {
            SQLiteCommand cmd = new SQLiteCommand(sql, Connection);

            try
            {
                cmd.Parameters.AddWithValue("@RateId", rates.RateId);
            }
            catch
            {
                cmd.Parameters.AddWithValue("@RateId", null);
            }
            try
            {
                cmd.Parameters.AddWithValue("@Code", rates.Code);
            }
            catch
            {
                cmd.Parameters.AddWithValue("@Code", "none");
            }
            try
            {
                cmd.Parameters.AddWithValue("@TaxRate", rates.TaxRate);
            }
            catch
            {
                cmd.Parameters.AddWithValue("@TaxRate", "none");
            }
            try
            {
                cmd.Parameters.AddWithValue("@Name", rates.Name);
            }
            catch
            {
                cmd.Parameters.AddWithValue("@Name", "none");
            }

            cmd.ExecuteNonQuery();
            Connection.Close();
        }
        
        #endregion

        #region Bitmap Access Methods

        /// <summary>
        /// Converts SVG into PNG
        /// </summary>
        /// <param name="svgDocument">Nuget Svg</param>
        /// <param name="name">Sytem.String</param>
        public static Flags ConvertSVG(SvgDocument svgDocument, string name)
        {
            Flags flag = new Flags();

            Bitmap bmp = svgDocument.Draw();
            bmp.Save($@"./Flags/{name}.png", ImageFormat.Png);
            flag.Flag = bmp;

            File.Delete($@"./Flags/{name}.svg");

            return flag;
        }

        /// <summary>
        /// Get flag
        /// </summary>
        /// <param name="country">Class country</param>
        /// <returns>Bitmap Image</returns>
        public static BitmapImage getFlag(Country country)
        {
            BitmapImage src = new BitmapImage();

            src.BeginInit();
            src.UriSource = new Uri($@".\Flags\{country.name}.png", UriKind.Relative);
            src.CacheOption = BitmapCacheOption.OnLoad;
            src.EndInit();
            

            return src;
        }

        #endregion
    }
}
