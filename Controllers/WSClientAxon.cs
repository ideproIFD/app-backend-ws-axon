using Microsoft.Ajax.Utilities;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceReference1;
using appbackendnetbankwebservice.Models;
using appbackendnetbankwebservice.Models.Dao;
using System.Collections.Generic;
using Nancy;

namespace appbackendnetbankwebservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WSClientAxonController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WSClientAxonController> _logger;

        public WSClientAxonController(ILogger<WSClientAxonController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            IeBankProxyServiceClient ieBankProxyServiceClient = new IeBankProxyServiceClient();

            int canal1 = 0;
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            DatosClienteRequest datosClienteRequest = new DatosClienteRequest { canal = 0, codigo_cliente = "10324120", codigo_apoderado = "10324120" };
            DatosClienteResponse datosClienteResponse = ieBankProxyServiceClient.DatosCliente(datosClienteRequest);

            List<Models.DatosCliente> ret = new List<Models.DatosCliente>();
            String datod = datosClienteResponse.DatosClienteResult;
            var listProductos = JsonConvert.DeserializeObject<List<Models.DatosCliente>>(datod);

            listProductos.ForEach(dto =>
            {
                var r = new Models.DatosCliente();
                r.nombre = dto.nombre;
                ret.Add(r);
            });





            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                dato123 = ret
            })
            .ToArray();
        }

        [HttpGet("datosCliente/{canal}/{codigo_cliente}/{codigo_apoderado}")]
        public async Task<IActionResult> GetDatosCliente(Int32 canal, string codigo_cliente, string codigo_apoderado)
        {
            IdeDatosClienteResponse response = new IdeDatosClienteResponse();
            IeBankProxyServiceClient ieBankProxyServiceClient = new IeBankProxyServiceClient();
            DatosClienteRequest datosClienteRequest = new DatosClienteRequest { canal = canal, codigo_cliente = codigo_cliente, codigo_apoderado = codigo_apoderado };
            DatosClienteResponse datosClienteResponse = ieBankProxyServiceClient.DatosCliente(datosClienteRequest);

            List<DatosCliente> list = new List<DatosCliente>();
            String ListClientResponse = datosClienteResponse.DatosClienteResult;
            var listaClientes = JsonConvert.DeserializeObject<List<DatosCliente>>(ListClientResponse);

            if (listaClientes.Count() > 0)
            {
                listaClientes.ForEach(value =>
                {
                    var dc = new DatosCliente();
                    dc.nombre = value.nombre.Trim();
                    dc.oficina = value.oficina.Trim();
                    dc.ejecutivo = value.ejecutivo.Trim();
                    dc.codigo_cliente = value.codigo_cliente.Trim();
                    dc.mail_oficial_cta = value.mail_oficial_cta.Trim();
                    dc.sucursal = value.sucursal;
                    dc.agencia = value.agencia;
                    list.Add(dc);
                });
                response.rows = list;
                response.total = list.Count();
                response.Message = "EXITO";
                response.Code = 200;
            }
            else
            {
                response.rows = list;
                response.total = list.Count();
                response.Message = "NO SE ENCONTRO REGISTROS";
                response.Code = 400;
            }

            var json = new JavaScriptSerializer().Serialize(response);
            return Ok(json);
        }


        [HttpGet("proximaCuotaCredito/{canal}/{codigo_cliente}/{codigo_apoderado}/{numero_credito}")]
        public async Task<IActionResult> proximaCuotaCredito(Int32 canal, string codigo_cliente, string codigo_apoderado, string numero_credito)
        {
            IdeProximaCuotaCreditoResponse response = new IdeProximaCuotaCreditoResponse();
            try
            {
                IeBankProxyServiceClient ieBankProxyServiceClient = new IeBankProxyServiceClient();
                proximaCuotaCreditoRequest ProximaCuotaRequest = new proximaCuotaCreditoRequest { canal = canal, codigoCliente = codigo_cliente, codigoApoderado = codigo_apoderado, numeroCredito = numero_credito };

                proximaCuotaCreditoRequest1 ProximaCuotaRequest1 = new proximaCuotaCreditoRequest1(ProximaCuotaRequest);
                proximaCuotaCreditoResponse1 proximaCuotaCreditoResponse = ieBankProxyServiceClient.proximaCuotaCredito(ProximaCuotaRequest1);


                List<ProximaCuotaCredito> list = new List<ProximaCuotaCredito>();
                var ListProximaCuotaResponse = proximaCuotaCreditoResponse.proximaCuotaCreditoResult;
                ProximaCuotaCredito pcc = new ProximaCuotaCredito();
                pcc.capital = ListProximaCuotaResponse.capital;
                pcc.diasMora = ListProximaCuotaResponse.diasMora;
                pcc.estado = ListProximaCuotaResponse.estado;
                pcc.fechaDeIncumplimiento = ListProximaCuotaResponse.fechaDeIncumplimiento;
                pcc.moneda = ListProximaCuotaResponse.moneda;
                pcc.montoDesembolsado = ListProximaCuotaResponse.montoDesembolsado;
                pcc.montoProximaCuota = ListProximaCuotaResponse.montoProximaCuota;
                pcc.numeroCredito = ListProximaCuotaResponse.numeroCredito;
                pcc.tipoCredito = ListProximaCuotaResponse.tipoCredito;
                list.Add(pcc);

                response.rows = list;
                response.total = list.Count();
                response.Message = "EXITO";
                response.Code = 200;
                var json = new JavaScriptSerializer().Serialize(response);
                return Ok(json);
            }
            catch (Exception ex)
            {
                response.Message = "INTERNAL SERVER ERROR";
                response.Code = 500;
                return Ok(response);
            }

        }


        [HttpPost("[action]")]
        public async Task<IActionResult> PagoCredito([FromBody] IdePagoCreditoRequest model)
        {
            IdePagoCreditoResponse response = new IdePagoCreditoResponse();
            try
            {
                IeBankProxyServiceClient ieBankProxyServiceClient = new IeBankProxyServiceClient();
                PagoCreditoRequest pagoCreditoRequest = new PagoCreditoRequest();
                pagoCreditoRequest.canal = model.canal;
                pagoCreditoRequest.codigo_cliente = model.codigo_cliente;
                pagoCreditoRequest.codigo_apoderado = model.codigo_apoderado;
                pagoCreditoRequest.numero_cc = model.numero_cc;
                pagoCreditoRequest.numero_credito = model.numero_credito;
                pagoCreditoRequest.fecha = model.fecha;
                pagoCreditoRequest.moneda = model.moneda;
                pagoCreditoRequest.signo_1 = model.signo_1;
                pagoCreditoRequest.monto_pago = model.monto_pago;

                PagoCreditoResponse pagoCreditoResponse = ieBankProxyServiceClient.PagoCredito(pagoCreditoRequest);
                List<RespuestaPagoCredito> list = new List<RespuestaPagoCredito>();
                String ListPagoRealizadoResponse = pagoCreditoResponse.PagoCreditoResult;
                var listaPagoRealizado = JsonConvert.DeserializeObject<List<RespuestaPagoCredito>>(ListPagoRealizadoResponse);

                if (listaPagoRealizado.Count() > 0)
                {
                    var error = listaPagoRealizado[0].error;

                    if (error == null)
                    {
                        listaPagoRealizado.ForEach(value =>
                        {
                            var dc = new RespuestaPagoCredito();
                            if (dc.error == null)
                            {
                                dc.numero_transaccion = value.numero_transaccion.Trim();
                                dc.fecha = value.fecha.Trim();
                                dc.signo_1 = value.signo_1.Trim();
                                dc.saldo_despues_pago = value.saldo_despues_pago.Trim();
                                list.Add(dc);
                            }
                        });
                        response.rows = list;
                        response.Message = "EXITO";
                        response.Code = 200;
                    } else
                    {
                        response.Message = listaPagoRealizado[0].mensaje;
                        response.Code = 403;
                    }
                }
                else
                {
                    response.Message = "NO SE ENCONTRO REGISTROS";
                    response.Code = 400;
                }
                return Ok(response);
            } catch (Exception ex)
            {
                response.Message = "INTERNAL SERVER ERROR";
                response.Code = 500;
                return Ok(response);
            }
        }
        
        // Metodo RevertirPagoCredito
        [HttpPost("[action]")]
        public async Task<IActionResult> RevertirPagoCredito([FromBody] IdeRevertirPagoCredito model)
        {
            IdeRevertirPagoCreditoResponse response = new IdeRevertirPagoCreditoResponse();
            try
            {
                IeBankProxyServiceClient ieBankProxyServiceClient = new IeBankProxyServiceClient();

                RequestRevertirPagoCredito revertirPagoCreditoRequet = new RequestRevertirPagoCredito();

                revertirPagoCreditoRequet.canal = model.canal;
                revertirPagoCreditoRequet.id_pago = model.id_pago;
                revertirPagoCreditoRequet.numero_credito = model.numero_credito;

                RevertirPagoCreditoRequest revertir = new RevertirPagoCreditoRequest(revertirPagoCreditoRequet);

                // RevertirPagoCreditoResponse pagoCreditoResponse = ieBankProxyServiceClient.PagoCredito(pagoCreditoRequest);


                RevertirPagoCreditoResponse revertirPagoCreditoResponse = ieBankProxyServiceClient.RevertirPagoCredito(revertir);

                var respRevertirPagoCredito = revertirPagoCreditoResponse.RevertirPagoCreditoResult;



                if (respRevertirPagoCredito.numero_credito is not null && respRevertirPagoCredito.numero_credito is not null)
                {

                    response.numero_credito = respRevertirPagoCredito.numero_credito;
                    response.Message = "EXITO";
                    response.Code = 200;
                }
                else
                {
                    response.Message = "NO SE ENCONTRO REGISTROS";
                    response.Code = 400;
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = "INTERNAL SERVER ERROR";
                response.Code = 500;
                return Ok(response);
            }
        }

    }
}