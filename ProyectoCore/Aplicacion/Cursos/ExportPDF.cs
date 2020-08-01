using iTextSharp.text;
using iTextSharp.text.pdf;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Cursos
{
    /// <summary>
    /// Clase para crear Reporte en PDF
    /// </summary>
    public class ExportPDF
    {
        public class Consulta : IRequest<Stream>
        {
        }

        public class Manejador : IRequestHandler<Consulta, Stream>
        {
            private readonly CursosOnlineContext _cursosOnlineContext;

            public Manejador(CursosOnlineContext cursosOnlineContext)
            {
                _cursosOnlineContext = cursosOnlineContext;
            }
            public async Task<Stream> Handle(Consulta request, CancellationToken cancellationToken)
            {
                Font fuenteTitulo = new Font(Font.HELVETICA, 8f, Font.BOLD, BaseColor.Blue); // Tipo de letra, tamaño de la letra, estilo de la letra, color de la letra
                Font fuenteHeader = new Font(Font.HELVETICA, 7f, Font.BOLD, BaseColor.Black); // Fuente cabecera tabla Cursos
                Font fuenteData = new Font(Font.HELVETICA, 7f, Font.NORMAL, BaseColor.Black); // Fuente datos cursos

                var cursos = await _cursosOnlineContext.Curso.ToListAsync();

                MemoryStream workStream = new MemoryStream();
                Rectangle rect = new Rectangle(PageSize.A4); // Definimos el tamaño del documento

                Document document = new Document(rect, 0, 0, 50, 100); // Defunimos el documento con sus margenes
                PdfWriter writer = PdfWriter.GetInstance(document, workStream); // Creamos la instancia para poder escribir dentro del archivo
                writer.CloseStream = false;

                document.Open(); // Abrimos el documento
                document.AddTitle("Lista de Cursos en la Universidad");

                PdfPTable tabla = new PdfPTable(1);
                tabla.WidthPercentage = 90; // Ancho
                PdfPCell celda = new PdfPCell(new Phrase("Lista de Cursos de SQL Server", fuenteTitulo));
                celda.Border = Rectangle.NO_BORDER; // Celdas sin bordes
                tabla.AddCell(celda);

                document.Add(tabla); // Agregamos la tabla al documento

                // Agregamos la tabla para mostrar los cursos
                // Header **********
                PdfPTable tablaCursos = new PdfPTable(2);
                float[] widths = new float[] { 40, 60 };
                tablaCursos.SetWidthPercentage(widths, rect);

                PdfPCell celdaHeaderTitulo = new PdfPCell(new Phrase("Curso", fuenteHeader));
                tablaCursos.AddCell(celdaHeaderTitulo);

                PdfPCell celdaHeaderDesc = new PdfPCell(new Phrase("Descripcion", fuenteHeader));
                tablaCursos.AddCell(celdaHeaderDesc);
                tablaCursos.WidthPercentage = 90;
                // ****************

                // Cuerpo data Cursos *****
                foreach (var cursoElemento in cursos)
                {
                    PdfPCell celdaDataTitulo = new PdfPCell(new Phrase(cursoElemento.Titulo, fuenteData));
                    tablaCursos.AddCell(celdaDataTitulo);

                    PdfPCell celdaDataDesc = new PdfPCell(new Phrase(cursoElemento.Descripcion, fuenteData));
                    tablaCursos.AddCell(celdaDataDesc);

                }
                // ************************

                document.Add(tablaCursos);

                document.Close();

                byte[] byteData = workStream.ToArray();
                workStream.Write(byteData, 0, byteData.Length);
                workStream.Position = 0;

                return workStream;
            }
        }
    }
}
