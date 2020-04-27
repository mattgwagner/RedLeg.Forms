namespace RedLeg.Forms
{
    /// <summary>
    /// This of course does not take into account gender non-binary soldiers.
    ///
    /// For administative purposes (i.e. scoring an APFT) we require a gender.
    /// </summary>
    public enum Gender : byte
    {
        Male = 0,

        Female = 1
    }
}