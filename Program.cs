using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using WebJobGxCGenesys.Models;
using DataTable = System.Data.DataTable;

namespace WebJobDinersHT1 {
    public class Program {

        public string strToken = "";
        public string inicio_proceso = "";
        public string fin_proceso = "";
        public string fecha_proceso = "";
        public string intervaloG = "";

        //Datos Oatuh GxC
        public static string clientId = "b35769b9-e2df-4138-a94d-01e76856dac9";
        public static string secretClientId = "ocMuH7ZnlR19rBrZq96A5MvgXW8qZbJC8Pe2EmhW7jY";
        public static string accessToken = "";
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage

        static void Main() {
            GetToken();
            DetalleInteracciones(); //carga detalle interacciones
        }

        public static void GetToken() {

            //Datos GxC
            clientId = "4606922a-121b-4fcd-a977-1156eefa127f";
            secretClientId = "_qcTx0sQMLjDHK-2wirDr26UArja026ASzNvTHBndpY";

            try {

                TokenPurecloud tempResponse = null;
                RestClient client011 = null;
                RestRequest request = null;
                IRestResponse response = null;
                while (tempResponse == null || tempResponse.access_token == "undefined" || tempResponse.access_token == null) {
                    client011 = new RestClient("https://login.usw2.pure.cloud/oauth/token?grant_type=client_credentials&client_id=" + clientId + "&client_secret=" + secretClientId);
                    client011.Timeout = -1;
                    request = new RestRequest(Method.POST);
                    request.AddHeader("Host", "login.usw2.pure.cloud");
                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Authorization", "Basic BASE64(" + clientId + ":" + secretClientId + ")");
                    request.JsonSerializer = new JsonDotNetSerializer();
                    response = client011.Execute(request);
                    tempResponse = JsonConvert.DeserializeObject<TokenPurecloud>(response.Content);
                }

                accessToken = tempResponse.access_token;

            } catch (Exception ee) {
                Console.WriteLine("Si existe: " + ee.Message);
                throw;
            }
        }//fin getToken

        public static void DetalleInteracciones() {
            //Se toma fecha de inicio de método
            DateTime a1 = DateTime.Now;

            int cont = 0;
            int contTH = 0;
            var totalhits = ""; ;

            //Creamos DataTable
            DataTable tbl = CreateDataTable();
            DataTable auxDataTable = new DataTable();

            string intervalo = string.Empty;
            string intervaloProceso = string.Empty;

            string jsonnewstedUU = string.Empty;
            Dictionary<string, string> IDAgentes = new Dictionary<string, string>();
            Dictionary<string, string> IDContactList = new Dictionary<string, string>();

            string URLRestAPI = "https://api." + "usw2.pure.cloud" + "/api/v2/users";

            double paginasA = 1;
            for (int ii = 1; ii <= paginasA; ii++) {

                URLRestAPI = "https://api.usw2.pure.cloud/api/v2/users?pageSize=100&pageNumber=" + ii;
                jsonnewstedUU = @"{'pageSize': 100,'pageNumber': '" + ii + "'}";

                var jsonsUU = JsonConvert.DeserializeObject(jsonnewstedUU);
                var clientU = new RestSharp.RestClient(URLRestAPI);
                var requestUU = new RestSharp.RestRequest(RestSharp.Method.GET);
                requestUU.RequestFormat = RestSharp.DataFormat.Json;
                requestUU.AddHeader("Content-Type", "application/json");
                requestUU.AddHeader("Host", "api.usw2.pure.cloud");
                requestUU.AddHeader("Authorization", "Bearer " + accessToken);
                requestUU.AddParameter("application/json", jsonsUU, ParameterType.RequestBody);
                IRestResponse responseUU = clientU.Execute(requestUU);
                RootObjectUser rootObjUU = JsonConvert.DeserializeObject<RootObjectUser>(responseUU.Content);

                var total = rootObjUU.total;
                double totalHistA = Convert.ToDouble(total);
                paginasA = Math.Round(totalHistA / 100);
                foreach (var item in rootObjUU.entities) {
                    IDAgentes.Add(item.id, item.name);
                }

            }

            //*************************************************************************************************
            URLRestAPI = "https://api." + "usw2.pure.cloud" + "/api/v2/outbound/contactlists/divisionviews?pageSize=100";

            double paginasACL = 1;
            for (int ii = 1; ii <= paginasACL; ii++) {
                URLRestAPI = "https://api.usw2.pure.cloud/api/v2/outbound/contactlists/divisionviews?pageSize=100&pageNumber=" + ii;
                jsonnewstedUU = @"{'pageSize': 100,'pageNumber': '" + ii + "'}";
                var jsonsUU = JsonConvert.DeserializeObject(jsonnewstedUU);
                var clientU = new RestSharp.RestClient(URLRestAPI);
                var requestUU = new RestSharp.RestRequest(RestSharp.Method.GET);
                requestUU.RequestFormat = RestSharp.DataFormat.Json;
                requestUU.AddHeader("Content-Type", "application/json");
                requestUU.AddHeader("Host", "api.usw2.pure.cloud");
                requestUU.AddHeader("Authorization", "Bearer " + accessToken);
                requestUU.AddParameter("application/json", jsonsUU, ParameterType.RequestBody);
                IRestResponse responseUU = clientU.Execute(requestUU);
                RootCL rootObjUUCL = JsonConvert.DeserializeObject<RootCL>(responseUU.Content);

                var total = rootObjUUCL.total;
                double totalHistA = Convert.ToDouble(total);
                paginasACL = Math.Round(totalHistA / 100);

                foreach (var item in rootObjUUCL.entities) {
                    IDContactList.Add(item.id, item.name);
                }

            }
            //*************************************************************************************************/


            //intervalo = CreaIntervalo30Min();
            //intervalo = CreaIntervaloDiario();
            intervalo = CreaIntervaloCallBacks();

            double paginasT = 1;

            //FECHA PROCESO
            //intervaloProceso = "2022-10-31T20:00:00.000Z/2022-10-31T21:00:00.000Z";
            intervaloProceso = intervalo;

            for (int ii = 1; ii <= paginasT; ii++) {

                string 
                //*
                jsonnewsted = @"{" +
                                    "'interval': '" + intervalo + "'," +
                                    "'order': 'asc'," +
                                    "'paging': {" +
                                        "'pageSize': 100," +
                                        "'pageNumber': '" + ii + "'" +
                                    "}" +
                                "}";
                //*/
                /*
                jsonnewsted = @"{" +
                                    "'interval': '" + intervalo + "'," +
                                    "'paging': { " +
                                        "'pageSize': 100," +
                                        "'pageNumber': '" + ii + "'" +
                                    "}," +
                                    //"'order': 'asc'," +
                                    //"'orderBy': 'conversationStart'," +
                                    "'order': 'desc'," +
                                    "'orderBy': 'segmentEnd'," +
                                    "'segmentFilters': [" +
                                        "{" +
                                            "'type': 'and'," +
                                            "'predicates': [" +
                                                "{" +
                                                 "'type': 'dimension'," +
                                                 "'dimension': 'segmentEnd'," +
                                                 $"'value': '{CreaIntervaloDiarioCallback()}'" +
                                                "}" +
                                            "]" +
                                        "}" +
                                    "]" +
                                "}";
                //*/

                Console.WriteLine($"jsonnewsted: {JsonConvert.SerializeObject(jsonnewsted, Formatting.None)}");
                //Console.WriteLine($"jsonnewsted: {jsonnewsted}");

                URLRestAPI = "https://api.usw2.pure.cloud/api/v2/analytics/conversations/details/query";
                var jsons = JsonConvert.DeserializeObject(jsonnewsted);

                var client = new RestClient(URLRestAPI);
                client.Timeout = -1;
                var request1 = new RestRequest(Method.POST);
                request1.RequestFormat = DataFormat.Json;
                request1.AddHeader("Content-Type", "application/json");
                request1.AddHeader("Authorization", "Bearer " + accessToken);
                request1.AddParameter("application/json", jsons, ParameterType.RequestBody);
                IRestResponse response1 = client.Execute(request1);

                RootObject rootObj = JsonConvert.DeserializeObject<RootObject>(response1.Content);

                totalhits = rootObj.totalHits;
                if (totalhits == null) totalhits = "0";

                Console.WriteLine($"totalhits: {totalhits}");

                double totalHistD = Convert.ToDouble(totalhits);
                paginasT = Math.Round(totalHistD / 100);
                System.Data.DataTable dt = new System.Data.DataTable();
                if (response1.Content != null && Int32.Parse(totalhits) > 0) {
                    if (response1.Content.Length > 0) {
                        try {
                            foreach (var item in rootObj.conversations) {
                                contTH++;
                                foreach (var item2 in item.participants) {
                                    foreach (var item3 in item2.sessions) {
                                        foreach (var item4 in item3.segments) {
                                            SqlParameter[] param = new SqlParameter[25];

                                            DateTime d1 = new DateTime();
                                            DateTime d2 = new DateTime();
                                            d1 = Convert.ToDateTime(item4.segmentStart.ToString("yyyy-MM-dd HH:mm:ss"));
                                            d2 = Convert.ToDateTime(item4.segmentEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                                            var diffInSeconds = (d1 - d2).TotalSeconds;

                                            var nombreusuario = Nonulo(item2.userId);
                                            var nombreusuarioF = IDAgentes.Where(p => p.Key == nombreusuario).FirstOrDefault().Value;
                                            var nombreContactList = Nonulo(item3.outboundContactListId);
                                            var nombreContactListF = IDContactList.Where(p => p.Key == nombreContactList).FirstOrDefault().Value;

                                            param[0] = new SqlParameter("@numId", 1);
                                            param[1] = new SqlParameter("@Fechaconsul", intervaloProceso);
                                            param[2] = new SqlParameter("@conversationId", Nonulo(item.conversationId));
                                            param[3] = new SqlParameter("@participantId", Nonulo(item2.participantId));
                                            param[4] = new SqlParameter("@participantName", Nonulo(item2.participantName));
                                            param[5] = new SqlParameter("@purpose", Nonulo(item2.purpose));
                                            param[6] = new SqlParameter("@sessionId", Nonulo(item3.sessionId));
                                            param[7] = new SqlParameter("@direction", Nonulo(item3.direction));
                                            param[8] = new SqlParameter("@dnis", Nonulo(item3.dnis));
                                            param[9] = new SqlParameter("@mediaType", Nonulo(item3.mediaType));
                                            param[10] = new SqlParameter("@timeoutSeconds", Nonulo(item3.timeoutSeconds));
                                            param[11] = new SqlParameter("@disconnectType", Nonulo(item4.disconnectType));
                                            param[12] = new SqlParameter("@segmentType", Nonulo(item4.segmentType));
                                            param[13] = new SqlParameter("@segmentStart", Nonulo(item4.segmentStart.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                                            param[14] = new SqlParameter("@segmentEnd", Nonulo(item4.segmentEnd.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                                            param[15] = new SqlParameter("@wrapUpCode", Nonulo(item4.wrapUpCode));
                                            param[16] = new SqlParameter("@userId", Nonulo(item2.userId));
                                            param[17] = new SqlParameter("@queueId", Nonulo(item4.queueId));
                                            param[18] = new SqlParameter("@outboundCampaignId", Nonulo(item3.outboundCampaignId));
                                            param[19] = new SqlParameter("@outboundContactId", Nonulo(item3.outboundContactId));
                                            param[20] = new SqlParameter("@outboundContactListId", Nonulo(nombreContactListF));
                                            param[21] = new SqlParameter("@Segundos", Math.Abs(diffInSeconds));
                                            param[22] = new SqlParameter("@ani", Nonulo(item3.ani));
                                            param[23] = new SqlParameter("@Usuario", Nonulo(nombreusuarioF));
                                            param[24] = new SqlParameter("@peerId", Nonulo(item3.peerId));


                                            Console.WriteLine($"Registro #{++cont}");
                                            auxDataTable = CargaData(tbl, param);

                                        }// end foreach segments
                                    }//end foreach sessions
                                }//end foreach participants
                            }//end for each conversations
                            Console.WriteLine($"Se ingresaron {contTH} hits");
                        }//end try
                        catch (Exception es) {
                            Console.Write("Exception when calling AnalyticsApi.PostAnalyticsConversationsDetailsQuery: " + es.Message);

                            var _log = Log(Convert.ToInt32(totalhits), contTH, cont, intervalo, $"Error: {es.Message}");
                            Console.Write($"Log: {_log}");
                        }
                    } else {
                        Console.Write($"No hay datos para procesar dentro del intervalo: {intervalo}");
                        var _log = Log(0, 0, 0, intervalo, $"No hay datos para procesar dentro del intervalo: {intervalo}");
                        Console.Write($"Log: {_log}");
                    }
                }
            }

            DateTime a2 = DateTime.Now;
            var estado = IngresaBase(auxDataTable);
            //var estado = "PRUEBA SIN INGRESAR";
            DateTime a3 = DateTime.Now;
            Console.WriteLine($"INICIO CARGA DATA = {a1}");
            Console.WriteLine($"INGRESA EN BASE = {a2}");
            Console.WriteLine($"FINALIZA = {a3}");

            var log = Log(Convert.ToInt32(totalhits), contTH, cont, intervalo, estado);
            Console.Write($"Log: {log}");

            Console.Write($"Proceso Finalizado dentro del intervalo: {intervalo}");
        }

        public static string CreaIntervalo30Min() {

            DateTime fecha = DateTime.Now;
            DateTime fecha1 = fecha;
            var ffc = fecha.ToString("yyyy-MM-dd");
            var ffn = fecha1.ToString("yyyy-MM-dd");

            //Fecha Inicio
            string anio = ffc.Substring(0, 4);
            string month = ffc.Substring(5, 2);
            string dia = ffc.Substring(8, 2);
            int hour = fecha.Hour;
            string fi = string.Empty;
            //-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-//


            //Fecha Fin
            string anio1 = ffn.Substring(0, 4);
            string month1 = ffn.Substring(5, 2);
            string dia1 = ffn.Substring(8, 2);
            int hour1 = fecha1.Hour;
            string ff = string.Empty;

            if (hour == 0) {
                if (fecha.Day == 1) {
                    var _auxDate = fecha.AddDays(-1);
                    //-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-//

                    int _anio = _auxDate.Year;
                    int _mes = _auxDate.Month;
                    int _dia = _auxDate.Day;

                    if (fecha1.Minute >= 0 && fecha1.Minute < 30) {
                        fi = $"{_anio}-{_mes}-{_dia}T23:30:00.000Z/";
                        ff = $"{anio1}-{month1}-{dia1}T00:00:0.000Z";
                    } else if (fecha1.Minute >= 30 && fecha1.Minute < 60) {
                        fi = $"{anio}-{month}-{dia}T{hour}:00:00.000Z/";
                        ff = $"{anio1}-{month1}-{dia1}T{hour1}:30:00.000Z";
                    }
                }
            } else {
                if (fecha1.Minute >= 0 && fecha1.Minute < 30) {
                    fi = $"{anio}-{month}-{dia}T{hour - 1}:30:00.000Z/";
                    ff = $"{anio1}-{month1}-{dia1}T{hour1}:00:00.000Z";
                } else if (fecha1.Minute >= 30 && fecha1.Minute < 60) {
                    fi = $"{anio}-{month}-{dia}T{hour}:00:00.000Z/";
                    ff = $"{anio1}-{month1}-{dia1}T{hour1}:30:00.000Z";
                }
            }

            string intervalo = fi + ff;
            return intervalo;
        }

        public static string CreaIntervaloDiario() {

            DateTime fecha = DateTime.Now.AddDays(1);
            //DateTime fecha = DateTime.Now;
            DateTime _auxDate = fecha.AddDays(-1);
            var ffc = fecha.ToString("yyyy-MM-dd");

            //Fecha Inicio Intervalo 1 dia menos al DateTime actual (yyyy-MM-dd)-1 05:00:00.000Z (00:00 utc-5)
            var _anio = _auxDate.Year;
            var _mes = _auxDate.Month;
            var _dia = _auxDate.Day;
            //-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-//
            //Fecha Fin Intervalo DateTime actual yyyy-MM-dd 05:00:00.000Z (00:00 utc-5)
            var anio = ffc.Substring(0, 4);
            var mes = ffc.Substring(5, 2);
            var dia = ffc.Substring(8, 2);
            //-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-//

            string intervalo = $"{_anio}-{_mes}-{_dia}T05:00:00.000Z/{anio}-{mes}-{dia}T05:00:00.000Z";

            return intervalo;
        }

        public static string CreaIntervaloDiarioCallback() {

            DateTime fecha = DateTime.Now.AddDays(1);
            //DateTime fecha = DateTime.Now;
            DateTime _auxDate = fecha.AddDays(-1);
            var ffc = fecha.ToString("yyyy-MM-dd");

            //Fecha Inicio Intervalo 1 dia menos al DateTime actual (yyyy-MM-dd)-1 05:00:00.000Z (00:00 utc-5)
            var _anio = _auxDate.Year;
            var _mes = _auxDate.Month;
            var _dia = _auxDate.Day;
            //-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-//
            //Fecha Fin Intervalo DateTime actual yyyy-MM-dd 05:00:00.000Z (00:00 utc-5)
            var anio = ffc.Substring(0, 4);
            var mes = ffc.Substring(5, 2);
            var dia = ffc.Substring(8, 2);
            //-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-//

            string intervalo = $"{_anio}-{_mes}-{_dia}T05:00:00.000Z/{anio}-{mes}-{dia}T05:00:00.000Z";

            return intervalo;
        }

        public static string CreaIntervaloCallBacks() {

            DateTime fecha = DateTime.Now.AddDays(1);
            DateTime _auxDate = fecha.AddDays(-29);


            //Fecha Inicio Intervalo 30 dias menos al DateTime actual (yyyy-MM-dd)-1 05:00:00.000Z (00:00 utc-5)
            var _anio = _auxDate.Year;
            var _mes = _auxDate.Month;
            var _dia = _auxDate.Day;
            //-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-//
            //Fecha Fin Intervalo DateTime actual yyyy-MM-dd 05:00:00.000Z (00:00 utc-5)
            var anio = fecha.Year;
            var mes = fecha.Month;
            var dia = fecha.Day;
            //-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-//

            string intervalo = $"{_anio}-{_mes}-{_dia}T05:00:00.000Z/{anio}-{mes}-{dia}T05:00:00.000Z";

            return intervalo;
        }

        public static string IngresaBase(DataTable dataTable) {
            string conString = "Server=tcp:srvdb-prod-mi.public.89feb080237b.database.windows.net,3342;Initial Catalog=Genesys;Persist Security Info=False;User ID=jaraujo;Password=JApass33;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=120;";

            List<string> colasIds = new List<string>();
            List<string> colasNombres = new List<string>();

            for (int i = 0; i < dataTable.Rows.Count; i++) {
                string aux = dataTable.Rows[i]["queueId"].ToString();

                if (!colasIds.Contains(aux)) colasIds.Add(aux);
            }

            for (int i = 0; i < colasIds.Count; i++) {
                string aux = GetNombreCola(colasIds[i]);
                colasNombres.Add(aux);
            }

            for (int i = 0; i < dataTable.Rows.Count; i++) {
                for (int j = 0; j < colasIds.Count; j++) {
                    if (colasIds[j].Equals(dataTable.Rows[i]["queueId"].ToString())) {
                        dataTable.Rows[i]["queueId"] = colasNombres[j];
                    }
                }
            }

            try {
                // connect to SQL
                using (SqlConnection connection = new SqlConnection(conString)) {
                    // make sure to enable triggers
                    // more on triggers in next post
                    SqlBulkCopy bulkCopy = new SqlBulkCopy(
                        connection,
                        SqlBulkCopyOptions.TableLock |
                        SqlBulkCopyOptions.FireTriggers |
                        SqlBulkCopyOptions.UseInternalTransaction,
                        null
                        );

                    // set the destination table name
                    bulkCopy.DestinationTableName = "tmp_DetalleInteracciones";
                    connection.Open();

                    // write the data in the "dataTable"
                    bulkCopy.WriteToServer(dataTable);
                    connection.Close();

                    // reset
                    dataTable.Clear();

                    return "Ejecución Exitosa";
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);

                return "Falló registro en base";
            }
        }

        public static string Nonulo(object dato) {
            try {

                if (dato == null || dato == "") {
                    return "0";
                } else {
                    return dato.ToString();
                }
            } catch (Exception e) {
                return e + "-  " + dato.ToString();
            }
        }

        public static DataTable CreateDataTable() {
            DataTable tbl = new DataTable();
            tbl.Columns.Add(new DataColumn("numId", typeof(string)));
            tbl.Columns.Add(new DataColumn("Fechaconsul", typeof(string)));
            tbl.Columns.Add(new DataColumn("conversationId", typeof(string)));
            tbl.Columns.Add(new DataColumn("participantId", typeof(string)));
            tbl.Columns.Add(new DataColumn("participantName", typeof(string)));
            tbl.Columns.Add(new DataColumn("purpose", typeof(string)));
            tbl.Columns.Add(new DataColumn("sessionId", typeof(string)));
            tbl.Columns.Add(new DataColumn("direction", typeof(string)));
            tbl.Columns.Add(new DataColumn("dnis", typeof(string)));
            tbl.Columns.Add(new DataColumn("mediaType", typeof(string)));
            tbl.Columns.Add(new DataColumn("timeoutSeconds", typeof(string)));
            tbl.Columns.Add(new DataColumn("disconnectType", typeof(string)));
            tbl.Columns.Add(new DataColumn("segmentType", typeof(string)));
            tbl.Columns.Add(new DataColumn("segmentStart", typeof(string)));
            tbl.Columns.Add(new DataColumn("segmentEnd", typeof(string)));
            tbl.Columns.Add(new DataColumn("wrapUpCode", typeof(string)));
            tbl.Columns.Add(new DataColumn("userId", typeof(string)));
            tbl.Columns.Add(new DataColumn("queueId", typeof(string)));
            tbl.Columns.Add(new DataColumn("outboundCampaignId", typeof(string)));
            tbl.Columns.Add(new DataColumn("outboundContactListId", typeof(string)));
            tbl.Columns.Add(new DataColumn("Segundos", typeof(string)));
            tbl.Columns.Add(new DataColumn("ani", typeof(string)));
            tbl.Columns.Add(new DataColumn("Usuario", typeof(string)));
            tbl.Columns.Add(new DataColumn("outboundContactId", typeof(string)));
            tbl.Columns.Add(new DataColumn("peerId", typeof(string)));

            return tbl;
        }

        public static DataTable CargaData(DataTable tbl, SqlParameter[] param) {
            DataRow dr = tbl.NewRow();

            DateTime d1 = Convert.ToDateTime(param[13].Value.ToString());
            DateTime d2 = Convert.ToDateTime(param[14].Value.ToString());
            DateTime myDate = DateTime.Parse("01/01/0001 0:00:00");

            if (d2 == myDate) {
                d2 = DateTime.Parse("01/01/1900 0:00:00");
            }

            dr["numId"] = param[0].Value.ToString();
            dr["Fechaconsul"] = param[1].Value.ToString();
            dr["conversationId"] = param[2].Value.ToString();
            dr["participantId"] = param[3].Value.ToString();
            dr["participantName"] = param[4].Value.ToString();
            dr["purpose"] = param[5].Value.ToString();
            dr["sessionId"] = param[6].Value.ToString();
            dr["direction"] = param[7].Value.ToString();
            dr["dnis"] = param[8].Value.ToString();
            dr["mediaType"] = param[9].Value.ToString();
            dr["timeoutSeconds"] = param[10].Value.ToString();
            dr["disconnectType"] = param[11].Value.ToString();
            dr["segmentType"] = param[12].Value.ToString();
            dr["segmentStart"] = d1;
            dr["segmentEnd"] = d2;
            dr["wrapUpCode"] = param[15].Value.ToString();
            dr["userId"] = param[16].Value.ToString();
            dr["queueId"] = param[17].Value.ToString();
            dr["outboundCampaignId"] = param[18].Value.ToString();
            dr["outboundContactId"] = param[19].Value.ToString();
            dr["ani"] = param[22].Value.ToString();
            dr["Segundos"] = param[21].Value.ToString();
            dr["Usuario"] = param[23].Value.ToString();
            dr["outboundContactListId"] = param[20].Value.ToString();
            dr["peerId"] = param[24].Value.ToString();
            
            tbl.Rows.Add(dr);

            return tbl;
        }

        public static string GetNombreCola(string queueId) {

            string URLRestAPI = "https://api.usw2.pure.cloud/api/v2/routing/queues/" + queueId;

            var clientU = new RestSharp.RestClient(URLRestAPI);
            var requestUU = new RestSharp.RestRequest(RestSharp.Method.GET);
            requestUU.RequestFormat = RestSharp.DataFormat.Json;
            requestUU.AddHeader("Content-Type", "application/json");
            requestUU.AddHeader("Host", "api.usw2.pure.cloud");
            requestUU.AddHeader("Authorization", "Bearer " + accessToken);

            IRestResponse responseUU = clientU.Execute(requestUU);
            Queue queue = JsonConvert.DeserializeObject<Queue>(responseUU.Content);

            if (string.IsNullOrEmpty(queue.Name)) {
                return $"No se encontro la cola {queueId}";
            } else {
                return queue.Name;
            }
        }

        public static string Log(int hitsRecibidos, int hitsIngresados, int registros, string intervaloConsulta, string estadoEjecucion) {
            DataTable tblLog = new DataTable();
            tblLog.Columns.Add(new DataColumn("registros_encontrados", typeof(int)));
            tblLog.Columns.Add(new DataColumn("registros_ingresados", typeof(int)));
            tblLog.Columns.Add(new DataColumn("registros_totales", typeof(int)));
            tblLog.Columns.Add(new DataColumn("intervalo", typeof(string)));
            tblLog.Columns.Add(new DataColumn("estado_ejecucion", typeof(string)));

            DataRow dr = tblLog.NewRow();
            dr["registros_encontrados"] = hitsRecibidos;
            dr["registros_ingresados"] = hitsIngresados;
            dr["registros_totales"] = registros;
            dr["intervalo"] = intervaloConsulta;
            dr["estado_ejecucion"] = estadoEjecucion;
            
            tblLog.Rows.Add(dr);

            string conString = "Server=tcp:srvdb-prod-mi.public.89feb080237b.database.windows.net,3342;Initial Catalog=Genesys;Persist Security Info=False;User ID=jaraujo;Password=JApass33;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=120;";

            try {
                // connect to SQL
                using (SqlConnection connection = new SqlConnection(conString)) {
                    // make sure to enable triggers
                    // more on triggers in next post
                    SqlBulkCopy bulkCopy = new SqlBulkCopy(
                        connection,
                        SqlBulkCopyOptions.TableLock |
                        SqlBulkCopyOptions.FireTriggers |
                        SqlBulkCopyOptions.UseInternalTransaction,
                        null
                        );

                    // set the destination table name
                    bulkCopy.DestinationTableName = "log_genesys";
                    connection.Open();

                    // write the data in the "dataTable"
                    bulkCopy.WriteToServer(tblLog);
                    connection.Close();

                    // reset
                    tblLog.Clear();
                    return "Ejecución Correcta";
                }
            } catch (Exception e) {
                return "Ejecución Incorrecta";
            }
        }
    }
}
