using QuestPDF.Fluent;
using Scopos.BabelFish.DataModel.OrionMatch;
using SkiaSharp;
using ZXing.QrCode;

namespace Scopos.BabelFish.DataActors.PDF {
    public class BarcodePdfGeneration {
        public static void GenerateQrLabelPdf( string outputPath, List<BarcodeLabelEncoding> barcodeData, int startPrintingAtRow = 0, int startPrintingAtColumn = 0 ) {
            const int labelsPerRow = 4;
            const int labelsPerColumn = 15;
            const int labelsPerPage = labelsPerRow * labelsPerColumn;

            // Page and label dimensions in points (1 inch = 72 points)
            const float pageWidth = 8.5f * 72;
            const float pageHeight = 11f * 72;
            const float labelWidth = 1.75f * 72;
            const float labelHeight = 0.666f * 72;
            const float cellPadding = .05f * 72;
            const int qrCodeDimension = (int)labelHeight - 5; // QR code fits within label height with padding
            const float marginTop = 0.505f * 72;
            const float marginBottom = 0.505f * 72;
            const float marginLeft = 0.3075f * 72;
            const float marginRight = 0.3075f * 72;
            const float horizontalSpacing = 0.295f * 72;
            const float verticalSpacing = 0f;
            const int maxCharacters = 18;
            const int padding = 4;

            int skipCells = startPrintingAtRow * labelsPerRow + startPrintingAtColumn;

            QuestPDF.Fluent.Document.Create( container => {
                container.Page( pageDescriptor => {
                    pageDescriptor.Size( pageWidth, pageHeight );
                    pageDescriptor.MarginLeft( marginLeft );
                    pageDescriptor.MarginRight( marginRight );
                    pageDescriptor.MarginTop( marginTop );
                    pageDescriptor.MarginBottom( marginBottom );

                    pageDescriptor.Content().Table( table => {
                        table.ColumnsDefinition( column => {
                            for (int colIndex = 0; colIndex < labelsPerRow; colIndex++) {
                                if (colIndex < labelsPerRow - 1) {
                                    column.ConstantColumn( labelWidth + horizontalSpacing );
                                } else {
                                    column.ConstantColumn( labelWidth );
                                }
                            }
                        } );

                        for (int skipIndex = 0; skipIndex < skipCells; skipIndex++) {
                            table.Cell().Border( 0 ).Padding( 0 ).Height( labelHeight );
                        }

                        foreach (var barcode in barcodeData) {
                            var qrBitmap = GenerateQrCodeBitmap( barcode.Encode, (int)qrCodeDimension );
                            table.Cell().Border( 0 ).Padding( 0 ).Height( labelHeight ).Row( cellRow => {

                                cellRow.ConstantItem( padding );
                                cellRow.ConstantItem( qrCodeDimension ).AlignMiddle().Padding( padding ).Image( qrBitmap ).FitArea();

                                cellRow.RelativeItem().AlignLeft().AlignMiddle().Padding( 0 ).PaddingLeft( cellPadding * 2 ).Text( text => {
                                    // Line 1 is the competitor's name, truncated to 18 characters to help fit on the label. This is to help the athlete know which label belongs to them.
                                    text.Line( Helpers.StringFormatting.GetTruncatedString( barcode.DisplayName, maxCharacters ) ).FontSize( 8 );

                                    // Line 2 is the competitor number, stage label, series, and course of fire ID. This is to help the athlete know which label belongs to them and where they are shooting.
                                    text.Line( $"{barcode.CompetitorNumber} {barcode.StageLabel}{barcode.Series} ({barcode.CourseOfFireId})" ).FontSize( 8 );

                                    // Line 3 is squadding, if available, otherwise left blank. This is to help the athlete know where they are shooting.
                                    if (!string.IsNullOrWhiteSpace( barcode.Squadding )) {
                                        text.Line( barcode.Squadding ).FontSize( 8 );
                                    }

                                    // Line 4 is either team name or club, if available, otherwise left blank. This is to help differentiate labels for sorting after printing.
                                    if (!string.IsNullOrWhiteSpace( barcode.TeamName )) {
                                        text.Line( Helpers.StringFormatting.GetTruncatedString( barcode.TeamName, maxCharacters ) ).FontSize( 8 );
                                    } else if (!string.IsNullOrWhiteSpace( barcode.Club )) {
                                        text.Line( Helpers.StringFormatting.GetTruncatedString( barcode.Club, maxCharacters ) ).FontSize( 8 );
                                    }
                                    text.ClampLines( 4 );
                                } );
                            } );

                        }
                    } );
                } );

            } ).GeneratePdf( outputPath );
        }

        // Helper to generate a QR code bitmap using ZXing.Net and SkiaSharp
        private static byte[] GenerateQrCodeBitmap( string text, int dimension ) {
            var writer = new ZXing.BarcodeWriterPixelData {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions {
                    Height = dimension,
                    Width = dimension,
                    Margin = 0,
                    ErrorCorrection = ZXing.QrCode.Internal.ErrorCorrectionLevel.M
                }
            };
            var pixelData = writer.Write( text );

            using (var image = new SKBitmap( pixelData.Width, pixelData.Height, SKColorType.Bgra8888, SKAlphaType.Premul )) {
                // Copy pixel data into SKBitmap
                System.Runtime.InteropServices.Marshal.Copy( pixelData.Pixels, 0, image.GetPixels(), pixelData.Pixels.Length );

                using (var ms = new MemoryStream())
                using (var skImage = SKImage.FromBitmap( image ))
                using (var data = skImage.Encode( SKEncodedImageFormat.Png, 100 )) {
                    data.SaveTo( ms );
                    return ms.ToArray();
                }
            }
        }
    }
}
