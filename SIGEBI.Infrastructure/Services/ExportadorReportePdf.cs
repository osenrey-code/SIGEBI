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
        private const string ColorPrimario = "#0F172A";      // Slate 900 (Moderno y profesional)
        private const string ColorSecundario = "#2563EB";    // Blue 600 (Acento vibrante)
        private const string ColorFondo = "#F8FAFC";         // Slate 50 (Fondo sutil para tarjetas)
        private const string ColorBorde = "#E2E8F0";         // Slate 200 (Bordes limpios)
        private const string ColorTexto = "#334155";         // Slate 700 (Texto legible)
        private const string ColorTextoOscuro = "#0F172A";   // Slate 900 (Títulos)
        private const string ColorTextoClaro = "#64748B";    // Slate 500 (Metadatos)
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
                            "Resumen general y métricas de préstamos registrados en el sistema"
                        )
                    );

                    page.Content().PaddingVertical(15).Column(column =>
                    {
                        column.Spacing(12);

                        column.Item().Row(row =>
                        {
                            row.Spacing(10);
                            row.RelativeItem().Element(container => CrearFechaGeneracion(container));
                            row.RelativeItem().Element(container => CrearRangoFechas(container, rango));
                        });

                        column.Item().Row(row =>
                        {
                            row.Spacing(10);

                            row.RelativeItem().Element(container =>
                                CrearTarjetaResumen(container, "Total préstamos", reporte.TotalPrestamos.ToString(), Colors.Blue.Darken2));

                            row.RelativeItem().Element(container =>
                                CrearTarjetaResumen(container, "A tiempo", reporte.PrestamosDevueltosATiempo.ToString(), Colors.Green.Darken2));

                            row.RelativeItem().Element(container =>
                                CrearTarjetaResumen(container, "Vencidos", reporte.PrestamosVencidos.ToString(), Colors.Red.Darken2));

                            row.RelativeItem().Element(container =>
                                CrearTarjetaResumen(container, "Puntualidad", $"{reporte.TasaDevolucionPuntual:N2}%", Colors.Indigo.Darken2));
                        });

                        column.Item().PaddingTop(5).Element(container =>
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
                                    columns.RelativeColumn(3); // Recurso (título más amplio)
                                    columns.RelativeColumn(1); // Ejemplar
                                    columns.ConstantColumn(75); // Inicio
                                    columns.ConstantColumn(75); // Límite
                                    columns.ConstantColumn(75); // Estado
                                });

                                table.Header(header =>
                                {
                                    CrearCeldaEncabezado(header, "Recurso");
                                    CrearCeldaEncabezado(header, "Ejemplar");
                                    CrearCeldaEncabezado(header, "Inicio");
                                    CrearCeldaEncabezado(header, "Límite");
                                    CrearCeldaEncabezado(header, "Estado");
                                });

                                foreach (var prestamo in reporte.Prestamos)
                                {
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
                            "Estado general y disponibilidad de los recursos físicos en existencia"
                        )
                    );

                    page.Content().PaddingVertical(15).Column(column =>
                    {
                        column.Spacing(12);

                        column.Item().Element(container => CrearFechaGeneracion(container));

                        column.Item().Row(row =>
                        {
                            row.Spacing(10);

                            row.RelativeItem().Element(container =>
                                CrearTarjetaResumen(container, "Recursos", reporte.TotalRecursos.ToString(), Colors.Blue.Darken2));

                            row.RelativeItem().Element(container =>
                                CrearTarjetaResumen(container, "Ejemplares", reporte.TotalEjemplares.ToString(), Colors.Grey.Darken2));

                            row.RelativeItem().Element(container =>
                                CrearTarjetaResumen(container, "Disponibles", reporte.EjemplaresDisponibles.ToString(), Colors.Green.Darken2));

                            row.RelativeItem().Element(container =>
                                CrearTarjetaResumen(container, "Fuera servicio", reporte.EjemplaresFueraDeServicio.ToString(), Colors.Red.Darken2));
                        });

                        column.Item().PaddingTop(5).Element(container =>
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
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(1);
                                    columns.ConstantColumn(50);
                                    columns.ConstantColumn(50);
                                    columns.ConstantColumn(50);
                                    columns.ConstantColumn(55);
                                });

                                table.Header(header =>
                                {
                                    CrearCeldaEncabezado(header, "Título");
                                    CrearCeldaEncabezado(header, "Categoría");
                                    CrearCeldaEncabezado(header, "Total");
                                    CrearCeldaEncabezado(header, "Disp.");
                                    CrearCeldaEncabezado(header, "Prest.");
                                    CrearCeldaEncabezado(header, "F.Serv.");
                                });

                                foreach (var recurso in reporte.Recursos)
                                {
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
                            "Análisis de recursos más solicitados y demanda por categoría"
                        )
                    );

                    page.Content().PaddingVertical(15).Column(column =>
                    {
                        column.Spacing(12);

                        column.Item().Row(row =>
                        {
                            row.Spacing(10);
                            row.RelativeItem().Element(container => CrearFechaGeneracion(container));
                            row.RelativeItem().Element(container => CrearRangoFechas(container, rango));
                        });

                        column.Item().Row(row =>
                        {
                            row.Spacing(10);

                            row.RelativeItem().Element(container =>
                                CrearTarjetaResumen(container, "Solicitudes", reporte.TotalSolicitudes.ToString(), Colors.Blue.Darken2));

                            row.RelativeItem().Element(container =>
                                CrearTarjetaResumen(container, "Top recursos", reporte.RecursosMasSolicitados.Count.ToString(), Colors.Indigo.Darken2));

                            row.RelativeItem().Element(container =>
                                CrearTarjetaResumen(container, "Categorías", reporte.DemandaPorCategoria.Count.ToString(), Colors.Teal.Darken2));

                            row.RelativeItem().Element(container =>
                                CrearTarjetaResumen(container, "Disponibilidad", $"{reporte.DisponibilidadPromedio:N2}%", Colors.Green.Darken2));
                        });

                        column.Item().PaddingTop(5).Element(container =>
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
                                    columns.RelativeColumn();
                                    columns.ConstantColumn(90);
                                });

                                table.Header(header =>
                                {
                                    CrearCeldaEncabezado(header, "Título");
                                    CrearCeldaEncabezado(header, "Solicitudes");
                                });

                                foreach (var recurso in reporte.RecursosMasSolicitados)
                                {
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
                                    columns.ConstantColumn(100);
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
                            "Resumen de sanciones, moras y distribución por tipo de usuario"
                        )
                    );

                    page.Content().PaddingVertical(15).Column(column =>
                    {
                        column.Spacing(12);

                        column.Item().Row(row =>
                        {
                            row.Spacing(10);
                            row.RelativeItem().Element(container => CrearFechaGeneracion(container));
                            row.RelativeItem().Element(container => CrearRangoFechas(container, rango));
                        });

                        column.Item().Row(row =>
                        {
                            row.Spacing(10);

                            row.RelativeItem().Element(container =>
                                CrearTarjetaResumen(container, "Total", reporte.TotalPenalizaciones.ToString(), Colors.Red.Darken2));

                            row.RelativeItem().Element(container =>
                                CrearTarjetaResumen(container, "Activas", reporte.PenalizacionesActivas.ToString(), Colors.Orange.Darken2));

                            row.RelativeItem().Element(container =>
                                CrearTarjetaResumen(container, "Resueltas", reporte.PenalizacionesResueltas.ToString(), Colors.Green.Darken2));

                            row.RelativeItem().Element(container =>
                                CrearTarjetaResumen(container, "Monto total", FormatearMonto(reporte.MontoTotalMora), Colors.Red.Darken4));
                        });

                        column.Item().PaddingTop(5).Element(container =>
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
                                    columns.ConstantColumn(75);
                                    columns.ConstantColumn(75);
                                    columns.ConstantColumn(75);
                                    columns.ConstantColumn(95);
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
                                    columns.RelativeColumn(1.5f); // Lector
                                    columns.ConstantColumn(85);   // Identificación
                                    columns.RelativeColumn(2f);   // Motivo
                                    columns.ConstantColumn(40);   // Días
                                    columns.ConstantColumn(75);   // Monto
                                    columns.ConstantColumn(65);   // Estado
                                });

                                table.Header(header =>
                                {
                                    CrearCeldaEncabezado(header, "Lector");
                                    CrearCeldaEncabezado(header, "Identificación");
                                    CrearCeldaEncabezado(header, "Motivo");
                                    CrearCeldaEncabezado(header, "Días");
                                    CrearCeldaEncabezado(header, "Monto");
                                    CrearCeldaEncabezado(header, "Estado");
                                });

                                foreach (var penalizacion in reporte.Detalles)
                                {
                                    CrearCeldaTexto(table, penalizacion.NombreUsuario);
                                    CrearCeldaTexto(table, penalizacion.IdentificacionUsuario);
                                    CrearCeldaTexto(table, RecortarTexto(penalizacion.Motivo, 50));
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
            page.Margin(25);
            page.PageColor(ColorBlanco);
            page.DefaultTextStyle(text => text.FontSize(9).FontColor(ColorTexto));
        }

        private static void CrearEncabezado(
            QPdfContainer container,
            string titulo,
            string subtitulo)
        {
            container
                .Background(ColorPrimario)
                .Padding(16)
                .Column(column =>
                {
                    column.Spacing(6);

                    column.Item().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("SIGEBI").FontSize(22).Bold().FontColor(ColorBlanco);
                            col.Item().Text(titulo).FontSize(13).SemiBold().FontColor("#38BDF8");
                        });

                        row.ConstantItem(120).AlignRight().AlignMiddle().Column(col =>
                        {
                            col.Item().Background("#1E293B").Padding(6).AlignCenter().Text("SISTEMA BIBLIOTECARIO")
                                .FontSize(7).Bold().FontColor("#94A3B8");
                        });
                    });

                    column.Item().PaddingTop(2).Text(subtitulo)
                        .FontSize(9)
                        .FontColor("#CBD5E1");
                });
        }

        private static void CrearFechaGeneracion(QPdfContainer container)
        {
            container
                .Background(ColorFondo)
                .Border(1)
                .BorderColor(ColorBorde)
                .Padding(8)
                .Row(row =>
                {
                    row.RelativeItem().Text("Generado por el Sistema de Gestión Bibliotecaria")
                        .FontSize(8)
                        .FontColor(ColorTextoClaro);

                    row.ConstantItem(130).AlignRight().Text($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}")
                        .FontSize(8)
                        .SemiBold()
                        .FontColor(ColorTextoOscuro);
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
                .Padding(8)
                .Text($"Periodo evaluado: {FormatearFecha(rango.FechaInicio)} - {FormatearFecha(rango.FechaFin)}")
                .FontSize(8)
                .SemiBold()
                .FontColor(ColorTextoOscuro);
        }

        private static void CrearTarjetaResumen(
            QPdfContainer container,
            string titulo,
            string valor,
            string colorValor)
        {
            container
                .Background(ColorFondo)
                .Border(1)
                .BorderColor(ColorBorde)
                .Padding(10)
                .Column(column =>
                {
                    column.Spacing(3);

                    column.Item()
                        .Text(titulo.ToUpperInvariant())
                        .FontSize(7)
                        .Bold()
                        .FontColor(ColorTextoClaro);

                    column.Item()
                        .Text(valor)
                        .FontSize(13)
                        .Bold()
                        .FontColor(colorValor);
                });
        }

        private static void CrearTituloSeccion(
            QPdfContainer container,
            string titulo)
        {
            container
                .PaddingTop(8)
                .PaddingBottom(4)
                .BorderBottom(1.5f)
                .BorderColor(ColorSecundario)
                .Text(titulo)
                .FontSize(11)
                .Bold()
                .FontColor(ColorPrimario);
        }

        private static void CrearMensajeSinDatos(QPdfContainer container)
        {
            container
                .Background("#FFFBEB")
                .Border(1)
                .BorderColor("#FCD34D")
                .Padding(10)
                .Text("No hay registros disponibles para mostrar en este reporte.")
                .FontSize(9)
                .FontColor("#92400E");
        }

        private static void CrearPiePagina(QPdfContainer container)
        {
            container
                .PaddingTop(10)
                .BorderTop(1)
                .BorderColor(ColorBorde)
                .Row(row =>
                {
                    row.RelativeItem()
                        .Text("SIGEBI - Plataforma de Gestión Bibliotecaria")
                        .FontSize(8)
                        .FontColor(ColorTextoClaro);

                    row.ConstantItem(120)
                        .AlignRight()
                        .Text(text =>
                        {
                            text.Span("Página ").FontSize(8).FontColor(ColorTextoClaro);
                            text.CurrentPageNumber().FontSize(8).Bold().FontColor(ColorTextoOscuro);
                            text.Span(" de ").FontSize(8).FontColor(ColorTextoClaro);
                            text.TotalPages().FontSize(8).Bold().FontColor(ColorTextoOscuro);
                        });
                });
        }

        private static void CrearCeldaEncabezado(
            TableCellDescriptor header,
            string texto)
        {
            header.Cell()
                .Background(ColorPrimario)
                .Border(1)
                .BorderColor(ColorPrimario)
                .PaddingVertical(6)
                .PaddingHorizontal(4)
                .Text(texto)
                .FontSize(8)
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
                .PaddingVertical(5)
                .PaddingHorizontal(4)
                .Text(string.IsNullOrWhiteSpace(texto) ? "N/A" : texto)
                .FontSize(8.5f)
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