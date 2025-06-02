using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Helpers {

    /// <summary>
    /// A series of helpful one off methods.
    /// </summary>
    public static class Common {

        /// <summary>
        /// Returns the Levenshtein Distance between two strings.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        /// <param name="caseSensitive"></param>
        /// <returns></returns>
        public static int LevenshteinDistance( string s, string t, bool caseSensitive = true ) {
            if (!caseSensitive) {
                s = s.ToLower();
                t = t.ToLower();
            }

            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0) {
                return m;
            }

            if (m == 0) {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++) {
            }

            for (int j = 0; j <= m; d[0, j] = j++) {
            }

            // Step 3
            for (int i = 1; i <= n; i++) {
                //Step 4
                for (int j = 1; j <= m; j++) {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min( d[i - 1, j] + 1, d[i, j - 1] + 1 ),
                        d[i - 1, j - 1] + cost );
                }
            }
            // Step 7
            return d[n, m];
        }


        private static Dictionary<string, int> forbiddenWordList = null;

        /// <summary>
        /// Returns a boolean indicating if the past in string is an offensive(ish) word in the english language.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool IsOffensiveEnglishWord( string word ) {

            if (forbiddenWordList == null) {
                forbiddenWordList = new Dictionary<string, int>();
                forbiddenWordList.Add( "ABBO", 1 );
                forbiddenWordList.Add( "ABO", 1 );
                forbiddenWordList.Add( "ABUSE", 1 );
                forbiddenWordList.Add( "ADDICT", 1 );
                forbiddenWordList.Add( "ADULT", 1 );
                forbiddenWordList.Add( "AFRICA", 1 );
                forbiddenWordList.Add( "ALLA", 1 );
                forbiddenWordList.Add( "ALLAH", 1 );
                forbiddenWordList.Add( "ANAL", 1 );
                forbiddenWordList.Add( "ANGIE", 1 );
                forbiddenWordList.Add( "ANGRY", 1 );
                forbiddenWordList.Add( "ANUS", 1 );
                forbiddenWordList.Add( "ARAB", 1 );
                forbiddenWordList.Add( "ARABS", 1 );
                forbiddenWordList.Add( "AREOLA", 1 );
                forbiddenWordList.Add( "ARGIE", 1 );
                forbiddenWordList.Add( "ARSE", 1 );
                forbiddenWordList.Add( "ASIAN", 1 );
                forbiddenWordList.Add( "ASS", 1 );
                forbiddenWordList.Add( "ASSES", 1 );
                forbiddenWordList.Add( "ASSHAT", 1 );
                forbiddenWordList.Add( "ASSMAN", 1 );
                forbiddenWordList.Add( "ATTACK", 1 );
                forbiddenWordList.Add( "BABE", 1 );
                forbiddenWordList.Add( "BABIES", 1 );
                forbiddenWordList.Add( "BALLS", 1 );
                forbiddenWordList.Add( "BARF", 1 );
                forbiddenWordList.Add( "BAST", 1 );
                forbiddenWordList.Add( "BEANER", 1 );
                forbiddenWordList.Add( "BEAST", 1 );
                forbiddenWordList.Add( "BEAVER", 1 );
                forbiddenWordList.Add( "BI", 1 );
                forbiddenWordList.Add( "BIATCH", 1 );
                forbiddenWordList.Add( "BIBLE", 1 );
                forbiddenWordList.Add( "BIGASS", 1 );
                forbiddenWordList.Add( "BIGGER", 1 );
                forbiddenWordList.Add( "BITCH", 1 );
                forbiddenWordList.Add( "BITCHY", 1 );
                forbiddenWordList.Add( "BITEME", 1 );
                forbiddenWordList.Add( "BLACK", 1 );
                forbiddenWordList.Add( "BLACKS", 1 );
                forbiddenWordList.Add( "BLIND", 1 );
                forbiddenWordList.Add( "BLOW", 1 );
                forbiddenWordList.Add( "BOANG", 1 );
                forbiddenWordList.Add( "BOGAN", 1 );
                forbiddenWordList.Add( "BOHUNK", 1 );
                forbiddenWordList.Add( "BOMB", 1 );
                forbiddenWordList.Add( "BOMBS", 1 );
                forbiddenWordList.Add( "BOMD", 1 );
                forbiddenWordList.Add( "BONER", 1 );
                forbiddenWordList.Add( "BONG", 1 );
                forbiddenWordList.Add( "BOOB", 1 );
                forbiddenWordList.Add( "BOOBS", 1 );
                forbiddenWordList.Add( "BOOBY", 1 );
                forbiddenWordList.Add( "BOODY", 1 );
                forbiddenWordList.Add( "BOOM", 1 );
                forbiddenWordList.Add( "BOONG", 1 );
                forbiddenWordList.Add( "BOONGA", 1 );
                forbiddenWordList.Add( "BOONIE", 1 );
                forbiddenWordList.Add( "BOOTY", 1 );
                forbiddenWordList.Add( "BRA", 1 );
                forbiddenWordList.Add( "BREA5T", 1 );
                forbiddenWordList.Add( "BREAST", 1 );
                forbiddenWordList.Add( "BUGGER", 1 );
                forbiddenWordList.Add( "BUNGA", 1 );
                forbiddenWordList.Add( "BURIED", 1 );
                forbiddenWordList.Add( "BURN", 1 );
                forbiddenWordList.Add( "BUTT", 1 );
                forbiddenWordList.Add( "BYATCH", 1 );
                forbiddenWordList.Add( "CACKER", 1 );
                forbiddenWordList.Add( "CANCER", 1 );
                forbiddenWordList.Add( "CHAV", 1 );
                forbiddenWordList.Add( "CHIN", 1 );
                forbiddenWordList.Add( "CHINK", 1 );
                forbiddenWordList.Add( "CHINKY", 1 );
                forbiddenWordList.Add( "CHOAD", 1 );
                forbiddenWordList.Add( "CHODE", 1 );
                forbiddenWordList.Add( "CHRIST", 1 );
                forbiddenWordList.Add( "CHURCH", 1 );
                forbiddenWordList.Add( "CIGS", 1 );
                forbiddenWordList.Add( "CLIT", 1 );
                forbiddenWordList.Add( "COCK", 1 );
                forbiddenWordList.Add( "COCKY", 1 );
                forbiddenWordList.Add( "COHEE", 1 );
                forbiddenWordList.Add( "COITUS", 1 );
                forbiddenWordList.Add( "COLOR", 1 );
                forbiddenWordList.Add( "COMMIE", 1 );
                forbiddenWordList.Add( "CONDOM", 1 );
                forbiddenWordList.Add( "COOLIE", 1 );
                forbiddenWordList.Add( "COOLY", 1 );
                forbiddenWordList.Add( "COON", 1 );
                forbiddenWordList.Add( "CRA5H", 1 );
                forbiddenWordList.Add( "CRABS", 1 );
                forbiddenWordList.Add( "CRACK", 1 );
                forbiddenWordList.Add( "CRAP", 1 );
                forbiddenWordList.Add( "CRAPPY", 1 );
                forbiddenWordList.Add( "CRASH", 1 );
                forbiddenWordList.Add( "CREAMY", 1 );
                forbiddenWordList.Add( "CRIME", 1 );
                forbiddenWordList.Add( "CRIMES", 1 );
                forbiddenWordList.Add( "CROTCH", 1 );
                forbiddenWordList.Add( "CUM", 1 );
                forbiddenWordList.Add( "CUMM", 1 );
                forbiddenWordList.Add( "CUMMER", 1 );
                forbiddenWordList.Add( "CUNN", 1 );
                forbiddenWordList.Add( "CUNNTT", 1 );
                forbiddenWordList.Add( "CUNT", 1 );
                forbiddenWordList.Add( "DAGO", 1 );
                forbiddenWordList.Add( "DAHMER", 1 );
                forbiddenWordList.Add( "DAMMIT", 1 );
                forbiddenWordList.Add( "DAMN", 1 );
                forbiddenWordList.Add( "DAMNIT", 1 );
                forbiddenWordList.Add( "DARKIE", 1 );
                forbiddenWordList.Add( "DARKY", 1 );
                forbiddenWordList.Add( "DEAD", 1 );
                forbiddenWordList.Add( "DEATH", 1 );
                forbiddenWordList.Add( "DEGO", 1 );
                forbiddenWordList.Add( "DEMON", 1 );
                forbiddenWordList.Add( "DESIRE", 1 );
                forbiddenWordList.Add( "DETH", 1 );
                forbiddenWordList.Add( "DEVIL", 1 );
                forbiddenWordList.Add( "DICK", 1 );
                forbiddenWordList.Add( "DIDDLE", 1 );
                forbiddenWordList.Add( "DIE", 1 );
                forbiddenWordList.Add( "DIED", 1 );
                forbiddenWordList.Add( "DIES", 1 );
                forbiddenWordList.Add( "DIKE", 1 );
                forbiddenWordList.Add( "DILDO", 1 );
                forbiddenWordList.Add( "DINK", 1 );
                forbiddenWordList.Add( "DIRTY", 1 );
                forbiddenWordList.Add( "DIVE", 1 );
                forbiddenWordList.Add( "DIX", 1 );
                forbiddenWordList.Add( "DONG", 1 );
                forbiddenWordList.Add( "DOODOO", 1 );
                forbiddenWordList.Add( "DOOM", 1 );
                forbiddenWordList.Add( "DOPE", 1 );
                forbiddenWordList.Add( "DRUG", 1 );
                forbiddenWordList.Add( "DRUNK", 1 );
                forbiddenWordList.Add( "DUMB", 1 );
                forbiddenWordList.Add( "DYEFLY", 1 );
                forbiddenWordList.Add( "DYKE", 1 );
                forbiddenWordList.Add( "EATME", 1 );
                forbiddenWordList.Add( "ENEMA", 1 );
                forbiddenWordList.Add( "ENEMY", 1 );
                forbiddenWordList.Add( "ERECT", 1 );
                forbiddenWordList.Add( "ERO", 1 );
                forbiddenWordList.Add( "ESCORT", 1 );
                forbiddenWordList.Add( "ETHNIC", 1 );
                forbiddenWordList.Add( "EVL", 1 );
                forbiddenWordList.Add( "FAECES", 1 );
                forbiddenWordList.Add( "FAG", 1 );
                forbiddenWordList.Add( "FAGGOT", 1 );
                forbiddenWordList.Add( "FAGOT", 1 );
                forbiddenWordList.Add( "FAILED", 1 );
                forbiddenWordList.Add( "FAIRY", 1 );
                forbiddenWordList.Add( "FAITH", 1 );
                forbiddenWordList.Add( "FART", 1 );
                forbiddenWordList.Add( "FARTY ", 1 );
                forbiddenWordList.Add( "FAT", 1 );
                forbiddenWordList.Add( "FATAH", 1 );
                forbiddenWordList.Add( "FATASS", 1 );
                forbiddenWordList.Add( "FATSO", 1 );
                forbiddenWordList.Add( "FCKCUM", 1 );
                forbiddenWordList.Add( "FEAR", 1 );
                forbiddenWordList.Add( "FECES", 1 );
                forbiddenWordList.Add( "FELCH", 1 );
                forbiddenWordList.Add( "FELTCH", 1 );
                forbiddenWordList.Add( "FETISH", 1 );
                forbiddenWordList.Add( "FIGHT", 1 );
                forbiddenWordList.Add( "FIRE", 1 );
                forbiddenWordList.Add( "FIRING", 1 );
                forbiddenWordList.Add( "FISTER", 1 );
                forbiddenWordList.Add( "FLANGE", 1 );
                forbiddenWordList.Add( "FLOO", 1 );
                forbiddenWordList.Add( "FLYDIE", 1 );
                forbiddenWordList.Add( "FLYDYE", 1 );
                forbiddenWordList.Add( "FOK", 1 );
                forbiddenWordList.Add( "FONDLE", 1 );
                forbiddenWordList.Add( "FORE", 1 );
                forbiddenWordList.Add( "FORNI", 1 );
                forbiddenWordList.Add( "FRAUD", 1 );
                forbiddenWordList.Add( "FU", 1 );
                forbiddenWordList.Add( "FUBAR", 1 );
                forbiddenWordList.Add( "FUC", 1 );
                forbiddenWordList.Add( "FUCCK", 1 );
                forbiddenWordList.Add( "FUCK", 1 );
                forbiddenWordList.Add( "FUCKA", 1 );
                forbiddenWordList.Add( "FUCKED", 1 );
                forbiddenWordList.Add( "FUCKER", 1 );
                forbiddenWordList.Add( "FUCKIN", 1 );
                forbiddenWordList.Add( "FUCKIT", 1 );
                forbiddenWordList.Add( "FUCKS", 1 );
                forbiddenWordList.Add( "FUGLY", 1 );
                forbiddenWordList.Add( "FUK", 1 );
                forbiddenWordList.Add( "FUKS", 1 );
                forbiddenWordList.Add( "FUNGUS", 1 );
                forbiddenWordList.Add( "FUUCK", 1 );
                forbiddenWordList.Add( "GAY", 1 );
                forbiddenWordList.Add( "GEEZ", 1 );
                forbiddenWordList.Add( "GEEZER", 1 );
                forbiddenWordList.Add( "GENI", 1 );
                forbiddenWordList.Add( "GERMAN", 1 );
                forbiddenWordList.Add( "GIN", 1 );
                forbiddenWordList.Add( "GINZO", 1 );
                forbiddenWordList.Add( "GIPP", 1 );
                forbiddenWordList.Add( "GIRLS", 1 );
                forbiddenWordList.Add( "GOB", 1 );
                forbiddenWordList.Add( "GOD", 1 );
                forbiddenWordList.Add( "GOOK", 1 );
                forbiddenWordList.Add( "GOY", 1 );
                forbiddenWordList.Add( "GOYIM", 1 );
                forbiddenWordList.Add( "GRINGO", 1 );
                forbiddenWordList.Add( "GROE", 1 );
                forbiddenWordList.Add( "GROSS", 1 );
                forbiddenWordList.Add( "GUBBA", 1 );
                forbiddenWordList.Add( "GUMMER", 1 );
                forbiddenWordList.Add( "GUN", 1 );
                forbiddenWordList.Add( "GYP", 1 );
                forbiddenWordList.Add( "GYPO", 1 );
                forbiddenWordList.Add( "GYPP", 1 );
                forbiddenWordList.Add( "GYPPIE", 1 );
                forbiddenWordList.Add( "GYPPO", 1 );
                forbiddenWordList.Add( "GYPPY", 1 );
                forbiddenWordList.Add( "HAMAS", 1 );
                forbiddenWordList.Add( "HAPA", 1 );
                forbiddenWordList.Add( "HARDER", 1 );
                forbiddenWordList.Add( "HARDON", 1 );
                forbiddenWordList.Add( "HAREM", 1 );
                forbiddenWordList.Add( "HEBE", 1 );
                forbiddenWordList.Add( "HEEB", 1 );
                forbiddenWordList.Add( "HELL", 1 );
                forbiddenWordList.Add( "HEROIN", 1 );
                forbiddenWordList.Add( "HERPES", 1 );
                forbiddenWordList.Add( "HIJACK", 1 );
                forbiddenWordList.Add( "HINDOO", 1 );
                forbiddenWordList.Add( "HITLER", 1 );
                forbiddenWordList.Add( "HIV", 1 );
                forbiddenWordList.Add( "HO", 1 );
                forbiddenWordList.Add( "HOBO", 1 );
                forbiddenWordList.Add( "HODGIE", 1 );
                forbiddenWordList.Add( "HOES", 1 );
                forbiddenWordList.Add( "HOLE", 1 );
                forbiddenWordList.Add( "HOMO", 1 );
                forbiddenWordList.Add( "HONGER", 1 );
                forbiddenWordList.Add( "HONK", 1 );
                forbiddenWordList.Add( "HONKEY", 1 );
                forbiddenWordList.Add( "HONKY", 1 );
                forbiddenWordList.Add( "HOOK", 1 );
                forbiddenWordList.Add( "HOOKER", 1 );
                forbiddenWordList.Add( "HORE", 1 );
                forbiddenWordList.Add( "HORK", 1 );
                forbiddenWordList.Add( "HORN", 1 );
                forbiddenWordList.Add( "HORNEY", 1 );
                forbiddenWordList.Add( "HORNY", 1 );
                forbiddenWordList.Add( "HOSER", 1 );
                forbiddenWordList.Add( "HUMMER", 1 );
                forbiddenWordList.Add( "HUSKY", 1 );
                forbiddenWordList.Add( "HUSSY", 1 );
                forbiddenWordList.Add( "HYMEN", 1 );
                forbiddenWordList.Add( "HYMIE", 1 );
                forbiddenWordList.Add( "IBLOWU", 1 );
                forbiddenWordList.Add( "IDIOT", 1 );
                forbiddenWordList.Add( "IKEY", 1 );
                forbiddenWordList.Add( "INCEST", 1 );
                forbiddenWordList.Add( "INSEST", 1 );
                forbiddenWordList.Add( "ISRAEL", 1 );
                forbiddenWordList.Add( "ITCH", 1 );
                forbiddenWordList.Add( "JADE", 1 );
                forbiddenWordList.Add( "JAP", 1 );
                forbiddenWordList.Add( "JEBUS", 1 );
                forbiddenWordList.Add( "JEEZ", 1 );
                forbiddenWordList.Add( "JESUS", 1 );
                forbiddenWordList.Add( "JEW", 1 );
                forbiddenWordList.Add( "JEWISH", 1 );
                forbiddenWordList.Add( "JIGA", 1 );
                forbiddenWordList.Add( "JIGG", 1 );
                forbiddenWordList.Add( "JIGGA", 1 );
                forbiddenWordList.Add( "JIGGY", 1 );
                forbiddenWordList.Add( "JIHAD", 1 );
                forbiddenWordList.Add( "JISM", 1 );
                forbiddenWordList.Add( "JIZ ", 1 );
                forbiddenWordList.Add( "JIZIM", 1 );
                forbiddenWordList.Add( "JIZM ", 1 );
                forbiddenWordList.Add( "JIZZ", 1 );
                forbiddenWordList.Add( "JIZZIM", 1 );
                forbiddenWordList.Add( "JIZZUM", 1 );
                forbiddenWordList.Add( "JOINT", 1 );
                forbiddenWordList.Add( "JUGS", 1 );
                forbiddenWordList.Add( "KAFFER", 1 );
                forbiddenWordList.Add( "KAFFIR", 1 );
                forbiddenWordList.Add( "KAFFRE", 1 );
                forbiddenWordList.Add( "KAFIR", 1 );
                forbiddenWordList.Add( "KANAKE", 1 );
                forbiddenWordList.Add( "KID", 1 );
                forbiddenWordList.Add( "KIGGER", 1 );
                forbiddenWordList.Add( "KIKE", 1 );
                forbiddenWordList.Add( "KILL", 1 );
                forbiddenWordList.Add( "KILLED", 1 );
                forbiddenWordList.Add( "KILLER", 1 );
                forbiddenWordList.Add( "KILLS", 1 );
                forbiddenWordList.Add( "KINK", 1 );
                forbiddenWordList.Add( "KINKY", 1 );
                forbiddenWordList.Add( "KKK", 1 );
                forbiddenWordList.Add( "KNIFE", 1 );
                forbiddenWordList.Add( "KOCK", 1 );
                forbiddenWordList.Add( "KONDUM", 1 );
                forbiddenWordList.Add( "KOON", 1 );
                forbiddenWordList.Add( "KOTEX", 1 );
                forbiddenWordList.Add( "KRAP", 1 );
                forbiddenWordList.Add( "KRAPPY", 1 );
                forbiddenWordList.Add( "KRAUT", 1 );
                forbiddenWordList.Add( "KUM", 1 );
                forbiddenWordList.Add( "KUMMER", 1 );
                forbiddenWordList.Add( "KUMS", 1 );
                forbiddenWordList.Add( "KUNT", 1 );
                forbiddenWordList.Add( "KY", 1 );
                forbiddenWordList.Add( "KYKE", 1 );
                forbiddenWordList.Add( "LAID", 1 );
                forbiddenWordList.Add( "LATIN", 1 );
                forbiddenWordList.Add( "LESBIN", 1 );
                forbiddenWordList.Add( "LESBO", 1 );
                forbiddenWordList.Add( "LEZ", 1 );
                forbiddenWordList.Add( "LEZBE", 1 );
                forbiddenWordList.Add( "LEZBO", 1 );
                forbiddenWordList.Add( "LEZZ", 1 );
                forbiddenWordList.Add( "LEZZO", 1 );
                forbiddenWordList.Add( "LIBIDO", 1 );
                forbiddenWordList.Add( "LICKER", 1 );
                forbiddenWordList.Add( "LICKME", 1 );
                forbiddenWordList.Add( "LIES", 1 );
                forbiddenWordList.Add( "LIMEY", 1 );
                forbiddenWordList.Add( "LIMY", 1 );
                forbiddenWordList.Add( "LIQUOR", 1 );
                forbiddenWordList.Add( "LOLITA", 1 );
                forbiddenWordList.Add( "LOOSER", 1 );
                forbiddenWordList.Add( "LOSER", 1 );
                forbiddenWordList.Add( "LOTION", 1 );
                forbiddenWordList.Add( "LSD", 1 );
                forbiddenWordList.Add( "LUGAN", 1 );
                forbiddenWordList.Add( "LYNCH", 1 );
                forbiddenWordList.Add( "MACACA", 1 );
                forbiddenWordList.Add( "MAD", 1 );
                forbiddenWordList.Add( "MAFIA", 1 );
                forbiddenWordList.Add( "MAGA", 1 );
                forbiddenWordList.Add( "MAMS", 1 );
                forbiddenWordList.Add( "METH", 1 );
                forbiddenWordList.Add( "MGGER", 1 );
                forbiddenWordList.Add( "MGGOR", 1 );
                forbiddenWordList.Add( "MILF", 1 );
                forbiddenWordList.Add( "MOCKEY", 1 );
                forbiddenWordList.Add( "MOCKIE", 1 );
                forbiddenWordList.Add( "MOCKY", 1 );
                forbiddenWordList.Add( "MOFO", 1 );
                forbiddenWordList.Add( "MOKY", 1 );
                forbiddenWordList.Add( "MOLES", 1 );
                forbiddenWordList.Add( "MOLEST", 1 );
                forbiddenWordList.Add( "MORMON", 1 );
                forbiddenWordList.Add( "MORON", 1 );
                forbiddenWordList.Add( "MOSLEM", 1 );
                forbiddenWordList.Add( "MUFF", 1 );
                forbiddenWordList.Add( "MUNT", 1 );
                forbiddenWordList.Add( "MURDER", 1 );
                forbiddenWordList.Add( "MUSLIM", 1 );
                forbiddenWordList.Add( "NAKED", 1 );
                forbiddenWordList.Add( "NASTY", 1 );
                forbiddenWordList.Add( "NAZI", 1 );
                forbiddenWordList.Add( "NECRO", 1 );
                forbiddenWordList.Add( "NEGRO", 1 );
                forbiddenWordList.Add( "NIG", 1 );
                forbiddenWordList.Add( "NIGER", 1 );
                forbiddenWordList.Add( "NIGG", 1 );
                forbiddenWordList.Add( "NIGGA", 1 );
                forbiddenWordList.Add( "NIGGAH", 1 );
                forbiddenWordList.Add( "NIGGAZ", 1 );
                forbiddenWordList.Add( "NIGGER", 1 );
                forbiddenWordList.Add( "NIGGLE", 1 );
                forbiddenWordList.Add( "NIGGOR", 1 );
                forbiddenWordList.Add( "NIGGUR", 1 );
                forbiddenWordList.Add( "NIGLET", 1 );
                forbiddenWordList.Add( "NIGNOG", 1 );
                forbiddenWordList.Add( "NIGR", 1 );
                forbiddenWordList.Add( "NIGRA", 1 );
                forbiddenWordList.Add( "NIGRE", 1 );
                forbiddenWordList.Add( "NIP", 1 );
                forbiddenWordList.Add( "NIPPLE", 1 );
                forbiddenWordList.Add( "NITTIT", 1 );
                forbiddenWordList.Add( "NLGGER", 1 );
                forbiddenWordList.Add( "NLGGOR", 1 );
                forbiddenWordList.Add( "NOOK", 1 );
                forbiddenWordList.Add( "NOOKEY", 1 );
                forbiddenWordList.Add( "NOOKIE", 1 );
                forbiddenWordList.Add( "NOONAN", 1 );
                forbiddenWordList.Add( "NOONER", 1 );
                forbiddenWordList.Add( "NUDE", 1 );
                forbiddenWordList.Add( "NUDGER", 1 );
                forbiddenWordList.Add( "NUKE", 1 );
                forbiddenWordList.Add( "NYMPH", 1 );
                forbiddenWordList.Add( "ORAL", 1 );
                forbiddenWordList.Add( "ORGA", 1 );
                forbiddenWordList.Add( "ORGASM", 1 );
                forbiddenWordList.Add( "ORGIES", 1 );
                forbiddenWordList.Add( "ORGY", 1 );
                forbiddenWordList.Add( "OSAMA", 1 );
                forbiddenWordList.Add( "PAKI", 1 );
                forbiddenWordList.Add( "PANSY", 1 );
                forbiddenWordList.Add( "PANTI", 1 );
                forbiddenWordList.Add( "PAYO", 1 );
                forbiddenWordList.Add( "PECK", 1 );
                forbiddenWordList.Add( "PECKER", 1 );
                forbiddenWordList.Add( "PEE", 1 );
                forbiddenWordList.Add( "PENDY", 1 );
                forbiddenWordList.Add( "PENI5", 1 );
                forbiddenWordList.Add( "PENILE", 1 );
                forbiddenWordList.Add( "PENIS", 1 );
                forbiddenWordList.Add( "PERIOD", 1 );
                forbiddenWordList.Add( "PERV", 1 );
                forbiddenWordList.Add( "PHUK", 1 );
                forbiddenWordList.Add( "PHUKED", 1 );
                forbiddenWordList.Add( "PHUQ", 1 );
                forbiddenWordList.Add( "PI55", 1 );
                forbiddenWordList.Add( "PIKER", 1 );
                forbiddenWordList.Add( "PIKEY", 1 );
                forbiddenWordList.Add( "PIKY", 1 );
                forbiddenWordList.Add( "PIMP", 1 );
                forbiddenWordList.Add( "PIMPED", 1 );
                forbiddenWordList.Add( "PIMPER", 1 );
                forbiddenWordList.Add( "PISS", 1 );
                forbiddenWordList.Add( "PISSED", 1 );
                forbiddenWordList.Add( "PISSER", 1 );
                forbiddenWordList.Add( "PISTOL", 1 );
                forbiddenWordList.Add( "PIXIE", 1 );
                forbiddenWordList.Add( "PIXY", 1 );
                forbiddenWordList.Add( "POCHA", 1 );
                forbiddenWordList.Add( "POCHO", 1 );
                forbiddenWordList.Add( "POHM", 1 );
                forbiddenWordList.Add( "POLACK", 1 );
                forbiddenWordList.Add( "POM", 1 );
                forbiddenWordList.Add( "POMMIE", 1 );
                forbiddenWordList.Add( "POMMY", 1 );
                forbiddenWordList.Add( "POO", 1 );
                forbiddenWordList.Add( "POON", 1 );
                forbiddenWordList.Add( "POOP", 1 );
                forbiddenWordList.Add( "POOPER", 1 );
                forbiddenWordList.Add( "POPIMP", 1 );
                forbiddenWordList.Add( "PORN", 1 );
                forbiddenWordList.Add( "PORNO", 1 );
                forbiddenWordList.Add( "POT", 1 );
                forbiddenWordList.Add( "PRIC", 1 );
                forbiddenWordList.Add( "PRICK", 1 );
                forbiddenWordList.Add( "PROS", 1 );
                forbiddenWordList.Add( "PU55I", 1 );
                forbiddenWordList.Add( "PU55Y", 1 );
                forbiddenWordList.Add( "PUBE", 1 );
                forbiddenWordList.Add( "PUBIC", 1 );
                forbiddenWordList.Add( "PUD", 1 );
                forbiddenWordList.Add( "PUDBOY", 1 );
                forbiddenWordList.Add( "PUDD", 1 );
                forbiddenWordList.Add( "PUKE", 1 );
                forbiddenWordList.Add( "PUSS", 1 );
                forbiddenWordList.Add( "PUSSIE", 1 );
                forbiddenWordList.Add( "PUSSY", 1 );
                forbiddenWordList.Add( "PUSY", 1 );
                forbiddenWordList.Add( "QUEEF", 1 );
                forbiddenWordList.Add( "QUEER", 1 );
                forbiddenWordList.Add( "QUIM", 1 );
                forbiddenWordList.Add( "RA8S", 1 );
                forbiddenWordList.Add( "RABBI", 1 );
                forbiddenWordList.Add( "RACIAL", 1 );
                forbiddenWordList.Add( "RACIST", 1 );
                forbiddenWordList.Add( "RANDY", 1 );
                forbiddenWordList.Add( "RAPE", 1 );
                forbiddenWordList.Add( "RAPED", 1 );
                forbiddenWordList.Add( "RAPER", 1 );
                forbiddenWordList.Add( "RAPIST", 1 );
                forbiddenWordList.Add( "RECTUM", 1 );
                forbiddenWordList.Add( "REEFER", 1 );
                forbiddenWordList.Add( "REJECT", 1 );
                forbiddenWordList.Add( "RERE", 1 );
                forbiddenWordList.Add( "RETARD", 1 );
                forbiddenWordList.Add( "RIBBED", 1 );
                forbiddenWordList.Add( "RIGGER", 1 );
                forbiddenWordList.Add( "RIMJOB", 1 );
                forbiddenWordList.Add( "ROACH", 1 );
                forbiddenWordList.Add( "ROBBER", 1 );
                forbiddenWordList.Add( "RUMP", 1 );
                forbiddenWordList.Add( "RUSSKI", 1 );
                forbiddenWordList.Add( "SADIS", 1 );
                forbiddenWordList.Add( "SADOM", 1 );
                forbiddenWordList.Add( "SANDM", 1 );
                forbiddenWordList.Add( "SATAN", 1 );
                forbiddenWordList.Add( "SCAG", 1 );
                forbiddenWordList.Add( "SCAT", 1 );
                forbiddenWordList.Add( "SCREW", 1 );
                forbiddenWordList.Add( "SCUM", 1 );
                forbiddenWordList.Add( "SEMEN", 1 );
                forbiddenWordList.Add( "SEPPO", 1 );
                forbiddenWordList.Add( "SEX", 1 );
                forbiddenWordList.Add( "SEXED", 1 );
                forbiddenWordList.Add( "SEXING", 1 );
                forbiddenWordList.Add( "SEXPOT", 1 );
                forbiddenWordList.Add( "SEXTOY", 1 );
                forbiddenWordList.Add( "SEXUAL", 1 );
                forbiddenWordList.Add( "SEXY", 1 );
                forbiddenWordList.Add( "SHAG", 1 );
                forbiddenWordList.Add( "SHAT", 1 );
                forbiddenWordList.Add( "SHAV", 1 );
                forbiddenWordList.Add( "SHHIT", 1 );
                forbiddenWordList.Add( "SHIT", 1 );
                forbiddenWordList.Add( "SHITE", 1 );
                forbiddenWordList.Add( "SHITED", 1 );
                forbiddenWordList.Add( "SHITS", 1 );
                forbiddenWordList.Add( "SHOOT", 1 );
                forbiddenWordList.Add( "SICK", 1 );
                forbiddenWordList.Add( "SISSY", 1 );
                forbiddenWordList.Add( "SKANK", 1 );
                forbiddenWordList.Add( "SKANKY", 1 );
                forbiddenWordList.Add( "SKUM", 1 );
                forbiddenWordList.Add( "SLANT", 1 );
                forbiddenWordList.Add( "SLAV", 1 );
                forbiddenWordList.Add( "SLAVE", 1 );
                forbiddenWordList.Add( "SLIME", 1 );
                forbiddenWordList.Add( "SLOPEY", 1 );
                forbiddenWordList.Add( "SLOPY", 1 );
                forbiddenWordList.Add( "SLUT", 1 );
                forbiddenWordList.Add( "SLUTS", 1 );
                forbiddenWordList.Add( "SLUTT", 1 );
                forbiddenWordList.Add( "SLUTTY", 1 );
                forbiddenWordList.Add( "SMACK", 1 );
                forbiddenWordList.Add( "SMUT", 1 );
                forbiddenWordList.Add( "SNATCH", 1 );
                forbiddenWordList.Add( "SNIPER", 1 );
                forbiddenWordList.Add( "SNOT", 1 );
                forbiddenWordList.Add( "SOB", 1 );
                forbiddenWordList.Add( "SODOM", 1 );
                forbiddenWordList.Add( "SODOMY", 1 );
                forbiddenWordList.Add( "SOOTY", 1 );
                forbiddenWordList.Add( "SOS", 1 );
                forbiddenWordList.Add( "SOVIET", 1 );
                forbiddenWordList.Add( "SPANK", 1 );
                forbiddenWordList.Add( "SPERM", 1 );
                forbiddenWordList.Add( "SPIC", 1 );
                forbiddenWordList.Add( "SPICK", 1 );
                forbiddenWordList.Add( "SPIG", 1 );
                forbiddenWordList.Add( "SPIK", 1 );
                forbiddenWordList.Add( "SPIT", 1 );
                forbiddenWordList.Add( "SPOOGE", 1 );
                forbiddenWordList.Add( "SPUNK", 1 );
                forbiddenWordList.Add( "SPUNKY", 1 );
                forbiddenWordList.Add( "SQUAW", 1 );
                forbiddenWordList.Add( "STAGG", 1 );
                forbiddenWordList.Add( "STIFFY", 1 );
                forbiddenWordList.Add( "STROKE", 1 );
                forbiddenWordList.Add( "STUPID", 1 );
                forbiddenWordList.Add( "SUCK", 1 );
                forbiddenWordList.Add( "SUCKER", 1 );
                forbiddenWordList.Add( "SUCKME", 1 );
                forbiddenWordList.Add( "SWALOW", 1 );
                forbiddenWordList.Add( "TABOO", 1 );
                forbiddenWordList.Add( "TAFF", 1 );
                forbiddenWordList.Add( "TAMPON", 1 );
                forbiddenWordList.Add( "TANG", 1 );
                forbiddenWordList.Add( "TANTRA", 1 );
                forbiddenWordList.Add( "TARD", 1 );
                forbiddenWordList.Add( "TEAT", 1 );
                forbiddenWordList.Add( "TERROR", 1 );
                forbiddenWordList.Add( "TESTE", 1 );
                forbiddenWordList.Add( "TINKLE", 1 );
                forbiddenWordList.Add( "TIT", 1 );
                forbiddenWordList.Add( "TITJOB", 1 );
                forbiddenWordList.Add( "TITS", 1 );
                forbiddenWordList.Add( "TITTIE", 1 );
                forbiddenWordList.Add( "TITTY", 1 );
                forbiddenWordList.Add( "TNT", 1 );
                forbiddenWordList.Add( "TOILET", 1 );
                forbiddenWordList.Add( "TONGUE", 1 );
                forbiddenWordList.Add( "TORTUR", 1 );
                forbiddenWordList.Add( "TOSSER", 1 );
                forbiddenWordList.Add( "TRAMP", 1 );
                forbiddenWordList.Add( "TRUMP", 1 );
                forbiddenWordList.Add( "TRANNY", 1 );
                forbiddenWordList.Add( "TROJAN", 1 );
                forbiddenWordList.Add( "TROTS", 1 );
                forbiddenWordList.Add( "TURD", 1 );
                forbiddenWordList.Add( "TURNON", 1 );
                forbiddenWordList.Add( "TWAT", 1 );
                forbiddenWordList.Add( "TWINK", 1 );
                forbiddenWordList.Add( "UCK", 1 );
                forbiddenWordList.Add( "UK", 1 );
                forbiddenWordList.Add( "URINE", 1 );
                forbiddenWordList.Add( "USAMA", 1 );
                forbiddenWordList.Add( "UTERUS", 1 );
                forbiddenWordList.Add( "VAGINA", 1 );
                forbiddenWordList.Add( "VIBR", 1 );
                forbiddenWordList.Add( "VIRGIN", 1 );
                forbiddenWordList.Add( "VOMIT", 1 );
                forbiddenWordList.Add( "VULVA", 1 );
                forbiddenWordList.Add( "WAB", 1 );
                forbiddenWordList.Add( "WANK", 1 );
                forbiddenWordList.Add( "WANKER", 1 );
                forbiddenWordList.Add( "WEAPON", 1 );
                forbiddenWordList.Add( "WEENIE", 1 );
                forbiddenWordList.Add( "WEEWEE", 1 );
                forbiddenWordList.Add( "WETB", 1 );
                forbiddenWordList.Add( "WHASH", 1 );
                forbiddenWordList.Add( "WHIT", 1 );
                forbiddenWordList.Add( "WHITES", 1 );
                forbiddenWordList.Add( "WHITEY", 1 );
                forbiddenWordList.Add( "WHIZ", 1 );
                forbiddenWordList.Add( "WHOP", 1 );
                forbiddenWordList.Add( "WHORE", 1 );
                forbiddenWordList.Add( "WIGGER", 1 );
                forbiddenWordList.Add( "WILLIE", 1 );
                forbiddenWordList.Add( "WILLY", 1 );
                forbiddenWordList.Add( "WN", 1 );
                forbiddenWordList.Add( "WOG", 1 );
                forbiddenWordList.Add( "WOP", 1 );
                forbiddenWordList.Add( "WTF", 1 );
                forbiddenWordList.Add( "WUSS", 1 );
                forbiddenWordList.Add( "WUZZIE", 1 );
                forbiddenWordList.Add( "XTC", 1 );
                forbiddenWordList.Add( "XXX", 1 );
                forbiddenWordList.Add( "YANKEE", 1 );
                forbiddenWordList.Add( "ZIGABO", 1 );
            }

            return forbiddenWordList.ContainsKey( word.ToUpper() );
        }


        private static Dictionary<string, string> stateList = null;

        /// <summary>
        /// Dictionary of US State abbreviations and state names.
        /// </summary>
        public static Dictionary<string, string> USStateList {
            get {

                if (stateList != null)
                    return stateList;

                stateList = new Dictionary<string, string>();
                stateList[""] = "";
                stateList["AL"] = "Alabama";
                stateList["AK"] = "Alaska";
                stateList["AS"] = "American Samoa";
                stateList["AZ"] = "Arizona";
                stateList["AR"] = "Arkansas";
                stateList["CA"] = "California";
                stateList["CO"] = "Colorado";
                stateList["CT"] = "Connecticut";
                stateList["DE"] = "Delaware";
                stateList["DC"] = "District of Columbia";
                stateList["FM"] = "Federated States of Micronesia";
                stateList["FL"] = "Florida";
                stateList["GA"] = "Georgia";
                stateList["GU"] = "Guam";
                stateList["HI"] = "Hawaii";
                stateList["ID"] = "Idaho";
                stateList["IL"] = "Illinois";
                stateList["IN"] = "Indiana";
                stateList["IA"] = "Iowa";
                stateList["KS"] = "Kansas";
                stateList["KY"] = "Kentucky";
                stateList["LA"] = "Louisiana";
                stateList["ME"] = "Maine";
                stateList["MH"] = "Marshall Islands";
                stateList["MD"] = "Maryland";
                stateList["MA"] = "Massachusetts";
                stateList["MI"] = "Michigan";
                stateList["MN"] = "Minnesota";
                stateList["MS"] = "Mississippi";
                stateList["MO"] = "Missouri";
                stateList["MT"] = "Montana";
                stateList["NE"] = "Nebraska";
                stateList["NV"] = "Nevada";
                stateList["NH"] = "New Hampshire";
                stateList["NJ"] = "New Jersey";
                stateList["NM"] = "New Mexico";
                stateList["NY"] = "New York";
                stateList["NC"] = "North Carolina";
                stateList["ND"] = "North Dakota";
                stateList["MP"] = "Northern Mariana Islands";
                stateList["OH"] = "Ohio";
                stateList["OK"] = "Oklahoma";
                stateList["OR"] = "Oregon";
                stateList["PW"] = "Palau";
                stateList["PA"] = "Pennsylvania";
                stateList["PR"] = "Puerto Rico";
                stateList["RI"] = "Rhode Island";
                stateList["SC"] = "South Carolina";
                stateList["SD"] = "South Dakota";
                stateList["TN"] = "Tennessee";
                stateList["TX"] = "Texas";
                stateList["UT"] = "Utah";
                stateList["VT"] = "Vermont";
                stateList["VI"] = "Virgin Islands";
                stateList["VA"] = "Virginia";
                stateList["WA"] = "Washington";
                stateList["WV"] = "West Virginia";
                stateList["WI"] = "Wisconsin";
                stateList["WY"] = "Wyoming";
                return stateList;
            }
        }

        /// <summary>
        /// Takes in a full path to a file (including directory) as a string. Then replaces any
        /// illegal characters with a '-' mark, returning the new string.
        /// <para>If the file name, portion of the full path, has an "\" in it (a directory seperator)
        /// then the sanitization will not work, as the file name gets mis-interpreted. It is better
        /// instead to use the method SanitizePath()</para>
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
		public static string SanitizeFullFilenamePath( string fullFilePath ) {
			if (string.IsNullOrEmpty( fullFilePath ))
				throw new ArgumentException( "Path cannot be null or empty.", nameof( fullFilePath ) );

            var fileInfo = new FileInfo( fullFilePath );
            var fileName = fileInfo.Name;
            var directory = fileInfo.Directory.ToString();

            return $"{SanitizeDirectoryPath( directory )}\\{SanitizeFileName( fileName )}";
		}

		public static string SanitizeDirectoryPath( string directoryPath ) {
			if (string.IsNullOrEmpty( directoryPath ))
				throw new ArgumentException( "Path cannot be null or empty.", nameof( directoryPath ) );

			char[] invalidPathChars = Path.GetInvalidPathChars();

			return new string( directoryPath.Select( c => invalidPathChars.Contains( c ) ? '-' : c ).ToArray() );
		}

		public static string SanitizeFileName( string fileName ) {
			if (string.IsNullOrEmpty( fileName ))
				throw new ArgumentException( "Path cannot be null or empty.", nameof( fileName ) );

			char[] invalidFileNameChars = Path.GetInvalidFileNameChars();

			return new string( fileName.Select( c => invalidFileNameChars.Contains( c ) ? '-' : c ).ToArray() );
		}

        public static string SanitizePath( string directoryPath, string fileName ) {
			if (string.IsNullOrEmpty( directoryPath ))
				throw new ArgumentException( "Path cannot be null or empty.", nameof( directoryPath ) );
			if (string.IsNullOrEmpty( fileName ))
				throw new ArgumentException( "Path cannot be null or empty.", nameof( fileName ) );

			return $"{SanitizeDirectoryPath( directoryPath )}\\{SanitizeFileName( fileName )}";

		}

	}
}
