namespace Scopos.BabelFish.DataActors.Excel {

    /// <summary>
    /// Base abstract class for generating Excel files (or their byte array).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ExcelGenerator<T> {

        public abstract byte[] GenerateExcel( string? filePath = null );

    }
}
