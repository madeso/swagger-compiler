namespace SwaggerCompiler
{
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Utility class to combine a list of strings to a single string. If 1 string is passed the result is the string,
    /// if 2 strings are passed the result is: string 1 + final separator + string 2. If more than 2 strings are passed the final is:
    /// string 1 + [separator + string N-1] + final separator + string N. If no strings are passed, EMPTY is returned. Usually ", "
    /// is passed for the first separator, " and " is passed for the second and "None" is passed as the empty argument.
    /// </summary>
    public class StringListCombiner
    {
        /// <summary> The empty string </summary>
        private readonly string empty;

        /// <summary> The final separator </summary>
        private readonly string finalSeparator;

        /// <summary> The separator </summary>
        private readonly string separator;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringListCombiner"/> class. Sets all the variables
        /// </summary>
        /// <param name="separator">
        /// The separator 
        /// </param>
        /// <param name="finalSeparator">
        /// The final separator 
        /// </param>
        /// <param name="empty">
        /// The empty string 
        /// </param>
        public StringListCombiner( string separator, string finalSeparator, string empty )
        {
            this.separator = separator;
            this.finalSeparator = finalSeparator;
            this.empty = empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringListCombiner"/> class. Sets the separators, leaves empty as an empty string
        /// </summary>
        /// <param name="separator">
        /// The separator 
        /// </param>
        /// <param name="finalSeparator">
        /// The final separator 
        /// </param>
        public StringListCombiner( string separator, string finalSeparator )
        {
            this.separator = separator;
            this.finalSeparator = finalSeparator;
            this.empty = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringListCombiner"/> class. Sets separators to the same value and leaves empty as black
        /// </summary>
        /// <param name="separator">
        /// The separator AND final separator 
        /// </param>
        public StringListCombiner( string separator )
        {
            this.separator = separator;
            this.finalSeparator = separator;
            this.empty = string.Empty;
        }

        /// <summary> Gets a new StringListCombiner for arrays. </summary>
        public static StringListCombiner Array
        {
            get
            {
                return new StringListCombiner( ", ", ", ", string.Empty );
            }
        }

        /// <summary> Gets a new StringListCombiner for ANDing in the english language (useful for debugging) </summary>
        public static StringListCombiner EnglishAnd
        {
            get
            {
                return new StringListCombiner( ", ", " and ", "None" );
            }
        }

        /// <summary> Gets a new StringListCombiner for ORing in the english language (useful for debugging) </summary>
        public static StringListCombiner EnglishOr
        {
            get
            {
                return new StringListCombiner( ", ", " or " );
            }
        }

        /// <summary>
        /// Combines a list. 
        /// </summary>
        /// <param name="strings">
        /// The string list to combine 
        /// </param>
        /// <returns>
        /// The combined string 
        /// </returns>
        public string Combine( List<string> strings )
        {
            if( strings.Count == 0 )
            {
                return this.empty;
            }

            StringBuilder builder = new StringBuilder();
            for( int index = 0; index < strings.Count; ++index )
            {
                string value = strings[index];
                builder.Append( value );

                // if this item isnt the last one in the list
                if( strings.Count != index + 1 )
                {
                    string s = this.separator;
                    if( strings.Count == index + 2 )
                    {
                        s = this.finalSeparator;
                    }

                    builder.Append( s );
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Combines an array, running ToString to convert to strings. 
        /// </summary>
        /// <param name="input">
        /// The array to combine 
        /// </param>
        /// <returns>
        /// The combined string 
        /// </returns>
        public string CombineFromArray( params object[] input )
        {
            return this.CombineFromEnumerable( Functional.Map( input, x => x.ToString() ) );
        }

        /// <summary>
        /// Combines a enumerable. 
        /// </summary>
        /// <param name="input">
        /// The enumerable to combine 
        /// </param>
        /// <returns>
        /// The combined string 
        /// </returns>
        public string CombineFromEnumerable( IEnumerable<string> input )
        {
            return this.Combine( new List<string>( input ) );
        }
    }
}
