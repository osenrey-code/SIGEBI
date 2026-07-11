using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SIGEBI.Application.DTOs.Request;
using SIGEBI.Application.DTOs.Response.ReporteResponse;
using SIGEBI.Application.Interfaces.Service;
using System.Globalization;

using QDocument = QuestPDF.Fluent.Document;
using QPdfContainer = QuestPDF.Infrastructure.IContainer;

namespace SIGEBI.Infrastructure.Services
{
    public class ExportadorReportePdf : IExportadorReportePdf
    {
        private const string ColorPrimario = "#1E3A8A";
        private const string ColorSecundario = "#2563EB";
        private const string ColorFondo = "#F8FAFC";
        private const string ColorBorde = "#CBD5E1";
        private const string ColorTexto = "#0F172A";
        private const string ColorTextoClaro = "#64748B";
        private const string ColorBlanco = "#FFFFFF";

        public byte[] GenerarReportePrestamosPdf(
            ReportePrestamoResponse reporte,
            ReporteRangoFRequest rango)
        {
            return QDocument.Create(document =>
            {
                document.Page(page =>
                {
                    ConfigurarPagina(page);

                    page.Header().Element(container =>
                        CrearEncabezado(
                            container,
                            "REPORTE DE PRÉSTAMOS",
                            "Resumen general de préstamos registrados en el sistema"
                        )
                    );

                    page.Content().PaddingVertical(15).Column(column =>
                    {
                        column.Spacing(15);

                        column.Item().Element(container => CrearFechaGeneracion(container));

                        column.Item().Element(container =>
                            CrearRangoFechas(container, rango)
                        );

                        column.Item().Row(row =>
                        {
                            row.Spacing(10);

                            row.RelativeItem().Element(container =>
                                CrearTarjetaResumen(container, "Total préstamos", reporte.TotalPrestamos.ToString()));

                            row.RelativeItem().Element(container =>
                                CrearTarjetaResumen(container, "Devueltos a tiempo", reporte.PrestamosDevueltosATiempo.ToString()));

                            row.RelativeItem().Element(container =>
                                CrearTarjetaResumen(container, "Vencidos", reporte.PrestamosVencidos.ToString()));

                            row.RelativeItem().Element(container =>
                                CrearTarjetaResumen(container, "Puntualidad", $"{reporte.TasaDevolucionPuntual:N2}%"));
                        });

                        column.Item().Element(container =>
                            CrearTituloSeccion(container, "Detalle de préstamos"));

                        if (reporte.Prestamos == null || !reporte.Prestamos.Any())
                        {
                            column.Item().Element(container => CrearMensajeSinDatos(container));
                        }
                        else
                        {
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(55);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn();
                                    columns.ConstantColumn(80);
                                    columns.ConstantColumn(80);
                                    columns.ConstantColumn(80);
                                });

                                table.Header(header =>
                                {
                                    CrearCeldaEncabezado(header, "ID");
                                    CrearCeldaEncabezado(header, "Recurso");
                                    CrearCeldaEncabezado(header, "Ejemplar");
                                    CrearCeldaEncabezado(header, "Inicio");
                                    CrearCeldaEncabezado(header, "Límite");
                                    CrearCeldaEncabezado(header, "Estado");
                                });

                                foreach (var prestamo in reporte.Prestamos)
                                {
                                    CrearCeldaTexto(table, prestamo.PrestamoId.ToString());
                                    CrearCeldaTexto(table, prestamo.TituloRecurso);
                                    CrearCeldaTexto(table, prestamo.IdentificadorEjemplar);
                                    CrearCeldaTexto(table, FormatearFecha(prestamo.FechaPrestamo));
                                    CrearCeldaTexto(table, FormatearFecha(prestamo.FechaLimite));
                                    CrearCeldaTexto(table, prestamo.Estado);
                                }
                            });
                        }
                    });

                    page.Footer().Element(CrearPiePagina);
                });
            }).GeneratePdf();
        }

        public byte[] GenerarReporteInventarioPdf(ReporteInventarioResponse reporte)
        {
            return QDocument.Create(document =>
            {
                document.Page(page =>
                {
                    ConfigurarPagina(page);

                    page.Header().Element(container =>
                        CrearEncabezado(
                            container,
                            "REPORTE DE INVENTARIO",
                            "Estado general de los recursos físicos registrados"
                        )
                    );

                    page.Content().PaddingVertical(15).Column(column =>
                    {
                        column.Spacing(15);

                        column.Item().Element(container => CrearFechaGeneracion(container));

                        column.Item().Row(row =>
                        {
                            row.Spacing(10);

                            row.RelativeItem().Element(container =>
                                CrearTarjetaResumen(container, "Total recursos", reporte.TotalRecursos.ToString()));

                            row.RelativeItem().Element(container =>
                                CrearTarjetaResumen(container, "Total ejemplares", reporte.TotalEjemplares.ToString()));

                            row.RelativeItem().Element(container =>
                                CrearTarjetaResumen(container, "Disponibles", reporte.EjemplaresDisponibles.ToString()));

                            row.RelativeItem().Element(container =>
                                CrearTarjetaResumen(container, "Fuera servicio", reporte.EjemplaresFueraDeServicio.ToString()));
                        });

                        column.Item().Element(container =>
                            CrearTituloSeccion(container, "Detalle de inventario"));

                        if (reporte.Recursos == null || !reporte.Recursos.Any())
                        {
                            column.Item().Element(container => CrearMensajeSinDatos(container));
                        }
                        else
                        {
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(55);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn();
                                    columns.ConstantColumn(55);
                                    columns.ConstantColumn(55);
                                    columns.ConstantColumn(55);
                                    columns.ConstantColumn(55);
                                });

                                table.Header(header =>
                                {
                                    CrearCeldaEncabezado(header, "ID");
                                    CrearCeldaEncabezado(header, "Título");
                                    CrearCeldaEncabezado(header, "Categoría");
                                    CrearCeldaEncabezado(header, "Total");
                                    CrearCeldaEncabezado(header, "Disp.");
                                    CrearCeldaEncabezado(header, "Prest.");
                                    CrearCeldaEncabezado(header, "F. Serv.");
                                });

                                foreach (var recurso in reporte.Recursos)
                                {
                                    CrearCeldaTexto(table, recurso.RecursoBibliograficoId.ToString());
                                    CrearCeldaTexto(table, recurso.Titulo);
                                    CrearCeldaTexto(table, recurso.Categoria);
                                    CrearCeldaTexto(table, recurso.TotalEjemplares.ToString());
                                    CrearCeldaTexto(table, recurso.Disponibles.ToString());
                                    CrearCeldaTexto(table, recurso.Prestados.ToString());
                                    CrearCeldaTexto(table, recurso.FueraDeServicio.ToString());
                                }
                            });
                        }
                    });

                    page.Footer().Element(CrearPiePagina);
                });
            }).GeneratePdf();
        }

        public byte[] GenerarReporteUsoCatalogoPdf(
            ReporteUsoCatalogoResponse reporte,
            ReporteRangoFRequest rango)
        {
            return QDocument.Create(document =>
            {
                document.Page(page =>
                {
                    ConfigurarPagina(page);

                    page.Header().Element(container =>
                        CrearEncabezado(
                            container,
                            "REPORTE DE USO DEL CATÁLOGO",
                            "Recursos más solicitados y demanda por categoría"
                        )
                    );

                    page.Content().PaddingVertical(15).Column(column =>
                    {
                        column.Spacing(15);

                        column.Item().Element(container => CrearFechaGeneracion(container));

                        column.Item().Element(container =>
                            CrearRangoFechas(container, rango)
                        );

                        column.Item().Row(row =>
                        {
                            row.Spacing(10);

                            row.RelativeItem().Element(container =>
                                CrearTarjetaResumen(container, "Total solicitudes", reporte.TotalSolicitudes.ToString()));

                            row.RelativeItem().Element(container =>
                                CrearTarjetaResumen(container, "Recursos solicitados", reporte.RecursosMasSolicitados.Count.ToString()));

                            row.RelativeItem().Element(container =>
                                CrearTarjetaResumen(container, "Categorías", reporte.DemandaPorCategoria.Count.ToString()));

                            row.RelativeItem().Element(container =>
                                CrearTarjetaResumen(container, "Disponibilidad", $"{reporte.DisponibilidadPromedio:N2}%"));
                        });

                        column.Item().Element(container =>
                            CrearTituloSeccion(container, "Recursos más solicitados"));

                        if (reporte.RecursosMasSolicitados == null || !reporte.RecursosMasSolicitados.Any())
                        {
                            column.Item().Element(container => CrearMensajeSinDatos(container));
                        }
                        else
                        {
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(80);
                                    columns.RelativeColumn();
                                    columns.ConstantColumn(100);
                                });

                                table.Header(header =>
                                {
                                    CrearCeldaEncabezado(header, "Recurso ID");
                                    CrearCeldaEncabezado(header, "Título");
                                    CrearCeldaEncabezado(header, "Solicitudes");
                                });

                                foreach (var recurso in reporte.RecursosMasSolicitados)
                                {
                                    CrearCeldaTexto(table, recurso.RecursoBibliograficoId.ToString());
                                    CrearCeldaTexto(table, recurso.Titulo);
                                    CrearCeldaTexto(table, recurso.CantidadSolicitudes.ToString());
                                }
                            });
                        }

                        column.Item().PaddingTop(10).Element(container =>
                            CrearTituloSeccion(container, "Demanda por categoría"));

                        if (reporte.DemandaPorCategoria == null || !reporte.DemandaPorCategoria.Any())
                        {
                            column.Item().Element(container => CrearMensajeSinDatos(container));
                        }
                        else
                        {
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.ConstantColumn(120);
                                });

                                table.Header(header =>
                                {
                                    CrearCeldaEncabezado(header, "Categoría");
                                    CrearCeldaEncabezado(header, "Cantidad");
                                });

                                foreach (var categoria in reporte.DemandaPorCategoria)
                                {
                                    CrearCeldaTexto(table, categoria.Categoria);
                                    CrearCeldaTexto(table, categoria.CantidadSolicitada.ToString());
                                }
                            });
                        }
                    });

                    page.Footer().Element(CrearPiePagina);
                });
            }).GeneratePdf();
        }

        public byte[] GenerarReportePenalizacionesPdf(
            ReportePenalizacionesResponse reporte,
            ReporteRangoFRequest rango)
        {
            return QDocument.Create(document =>
            {
                document.Page(page =>
                {
                    ConfigurarPagina(page);

                    page.Header().Element(container =>
                        CrearEncabezado(
                            container,
                            "REPORTE DE PENALIZACIONES",
                            "Resumen de penalizaciones emitidas en el sistema"
                        )
                    );

                    page.Content().PaddingVertical(15).Column(column =>
                    {
                        column.Spacing(15);

                        column.Item().Element(container => CrearFechaGeneracion(container));

                        column.Item().Element(container =>
                            CrearRangoFechas(container, rango)
                        );

                        column.Item().Row(row =>
                        {
                            row.Spacing(10);

                            row.RelativeItem().Element(container =>
                                CrearTarjetaResumen(container, "Total", reporte.TotalPenalizaciones.ToString()));

                            row.RelativeItem().Element(container =>
                                CrearTarjetaResumen(container, "Activas", reporte.PenalizacionesActivas.ToString()));

                            row.RelativeItem().Element(container =>
                                CrearTarjetaResumen(container, "Resueltas", reporte.PenalizacionesResueltas.ToString()));

                            row.RelativeItem().Element(container =>
                                CrearTarjetaResumen(container, "Monto total", FormatearMonto(reporte.MontoTotalMora)));
                        });

                        column.Item().Element(container =>
                            CrearTituloSeccion(container, "Penalizaciones por tipo de usuario"));

                        if (reporte.PorTipoUsuario == null || !reporte.PorTipoUsuario.Any())
                        {
                            column.Item().Element(container => CrearMensajeSinDatos(container));
                        }
                        else
                        {
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.ConstantColumn(80);
                                    columns.ConstantColumn(80);
                                    columns.ConstantColumn(80);
                                    columns.ConstantColumn(90);
                                });

                                table.Header(header =>
                                {
                                    CrearCeldaEncabezado(header, "Tipo usuario");
                                    CrearCeldaEncabezado(header, "Generadas");
                                    CrearCeldaEncabezado(header, "Activas");
                                    CrearCeldaEncabezado(header, "Resueltas");
                                    CrearCeldaEncabezado(header, "Monto");
                                });

                                foreach (var item in reporte.PorTipoUsuario)
                                {
                                    CrearCeldaTexto(table, item.TipoUsuario);
                                    CrearCeldaTexto(table, item.Generadas.ToString());
                                    CrearCeldaTexto(table, item.Activas.ToString());
                                    CrearCeldaTexto(table, item.Resueltas.ToString());
                                    CrearCeldaTexto(table, FormatearMonto(item.MontoTotal));
                                }
                            });
                        }

                        column.Item().PaddingTop(10).Element(container =>
                            CrearTituloSeccion(container, "Detalle de penalizaciones"));

                        if (reporte.Detalles == null || !reporte.Detalles.Any())
                        {
                            column.Item().Element(container => CrearMensajeSinDatos(container));
                        }
                        else
                        {
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(55);
                                    columns.ConstantColumn(65);
                                    columns.RelativeColumn();
                                    columns.ConstantColumn(55);
                                    columns.ConstantColumn(85);
                                    columns.ConstantColumn(75);
                                });

                                table.Header(header =>
                                {
                                    CrearCeldaEncabezado(header, "ID");
                                    CrearCeldaEncabezado(header, "Usuario");
                                    CrearCeldaEncabezado(header, "Motivo");
                                    CrearCeldaEncabezado(header, "Días");
                                    CrearCeldaEncabezado(header, "Monto");
                                    CrearCeldaEncabezado(header, "Estado");
                                });

                                foreach (var penalizacion in reporte.Detalles)
                                {
                                    CrearCeldaTexto(table, penalizacion.PenalizacionId.ToString());
                                    CrearCeldaTexto(table, penalizacion.UsuarioId.ToString());
                                    CrearCeldaTexto(table, RecortarTexto(penalizacion.Motivo, 80));
                                    CrearCeldaTexto(table, penalizacion.DiasRetraso.ToString());
                                    CrearCeldaTexto(table, FormatearMonto(penalizacion.MontoMora));
                                    CrearCeldaTexto(table, penalizacion.Estado);
                                }
                            });
                        }
                    });

                    page.Footer().Element(CrearPiePagina);
                });
            }).GeneratePdf();
        }

        private static void ConfigurarPagina(PageDescriptor page)
        {
            page.Size(PageSizes.A4);
            page.Margin(30);
            page.PageColor(ColorBlanco);
            page.DefaultTextStyle(text => text.FontSize(10).FontColor(ColorTexto));
        }

        private static void CrearEncabezado(
            QPdfContainer container,
            string titulo,
            string subtitulo)
        {
            container
                .Background(ColorPrimario)
                .Padding(18)
                .Column(column =>
                {
                    column.Spacing(4);

                    column.Item()
                        .Text("SIGEBI")
                        .FontSize(26)
                        .Bold()
                        .FontColor(ColorBlanco);

                    column.Item()
                        .Text(titulo)
                        .FontSize(16)
                        .SemiBold()
                        .FontColor(ColorBlanco);

                    column.Item()
                        .Text(subtitulo)
                        .FontSize(10)
                        .FontColor("#DBEAFE");
                });
        }

        private static void CrearFechaGeneracion(QPdfContainer container)
        {
            container
                .Background(ColorFondo)
                .Border(1)
                .BorderColor(ColorBorde)
                .Padding(10)
                .Row(row =>
                {
                    row.RelativeItem()
                        .Text("Documento generado automáticamente por el Sistema de Gestión Bibliotecaria")
                        .FontSize(9)
                        .FontColor(ColorTextoClaro);

                    row.ConstantItem(160)
                        .AlignRight()
                        .Text($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}")
                        .FontSize(9)
                        .SemiBold()
                        .FontColor(ColorTexto);
                });
        }

        private static void CrearRangoFechas(
            QPdfContainer container,
            ReporteRangoFRequest rango)
        {
            container
                .Background(ColorFondo)
                .Border(1)
                .BorderColor(ColorBorde)
                .Padding(10)
                .Text($"Periodo: {FormatearFecha(rango.FechaInicio)} - {FormatearFecha(rango.FechaFin)}")
                .FontSize(9)
                .SemiBold()
                .FontColor(ColorTexto);
        }

        private static void CrearTarjetaResumen(
            QPdfContainer container,
            string titulo,
            string valor)
        {
            container
                .Background(ColorFondo)
                .Border(1)
                .BorderColor(ColorBorde)
                .Padding(12)
                .Column(column =>
                {
                    column.Spacing(5);

                    column.Item()
                        .Text(titulo)
                        .FontSize(9)
                        .FontColor(ColorTextoClaro);

                    column.Item()
                        .Text(valor)
                        .FontSize(15)
                        .Bold()
                        .FontColor(ColorPrimario);
                });
        }

        private static void CrearTituloSeccion(
            QPdfContainer container,
            string titulo)
        {
            container
                .PaddingTop(5)
                .PaddingBottom(5)
                .BorderBottom(1)
                .BorderColor(ColorBorde)
                .Text(titulo)
                .FontSize(13)
                .Bold()
                .FontColor(ColorPrimario);
        }

        private static void CrearMensajeSinDatos(QPdfContainer container)
        {
            container
                .Background("#FEF3C7")
                .Border(1)
                .BorderColor("#F59E0B")
                .Padding(12)
                .Text("No hay datos disponibles para este reporte.")
                .FontSize(10)
                .FontColor("#92400E");
        }

        private static void CrearPiePagina(QPdfContainer container)
        {
            container
                .BorderTop(1)
                .BorderColor(ColorBorde)
                .PaddingTop(8)
                .Row(row =>
                {
                    row.RelativeItem()
                        .Text("SIGEBI - Sistema de Gestión Bibliotecaria")
                        .FontSize(9)
                        .FontColor(ColorTextoClaro);

                    row.ConstantItem(120)
                        .AlignRight()
                        .Text(text =>
                        {
                            text.Span("Página ").FontSize(9).FontColor(ColorTextoClaro);
                            text.CurrentPageNumber().FontSize(9).FontColor(ColorTextoClaro);
                            text.Span(" de ").FontSize(9).FontColor(ColorTextoClaro);
                            text.TotalPages().FontSize(9).FontColor(ColorTextoClaro);
                        });
                });
        }

        private static void CrearCeldaEncabezado(
            TableCellDescriptor header,
            string texto)
        {
            header.Cell()
                .Background(ColorSecundario)
                .Border(1)
                .BorderColor(ColorSecundario)
                .PaddingVertical(7)
                .PaddingHorizontal(5)
                .Text(texto)
                .FontSize(9)
                .Bold()
                .FontColor(ColorBlanco);
        }

        private static void CrearCeldaTexto(
            TableDescriptor table,
            string? texto)
        {
            table.Cell()
                .BorderBottom(1)
                .BorderColor(ColorBorde)
                .PaddingVertical(6)
                .PaddingHorizontal(5)
                .Text(string.IsNullOrWhiteSpace(texto) ? "N/A" : texto)
                .FontSize(9)
                .FontColor(ColorTexto);
        }

        private static string FormatearFecha(DateTime fecha)
        {
            return fecha.ToString("dd/MM/yyyy");
        }

        private static string FormatearMonto(decimal monto)
        {
            return $"RD$ {monto.ToString("N2", CultureInfo.InvariantCulture)}";
        }

        private static string RecortarTexto(string? texto, int longitudMaxima)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return "N/A";

            texto = texto.Trim();

            if (texto.Length <= longitudMaxima)
                return texto;

            return texto.Substring(0, longitudMaxima) + "...";
        }
    }
}