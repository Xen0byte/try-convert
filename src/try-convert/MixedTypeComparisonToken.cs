﻿namespace MSBuild.Conversion
{
    internal enum VersionSubtokenType
    {
        Unknown = 0,
        Numeric,
        String
    }

    internal class MixedTypeComparisonToken
    {
        public MixedTypeComparisonToken(string rawToken)
        {
            _rawToken = rawToken;
            _tokenType = VersionSubtokenType.Unknown;
        }

        private VersionSubtokenType _tokenType;
        private int _numericValue;
        private string _stringValue;
        private string _rawToken;

        private void EnsureParsed()
        {
            if (_tokenType == VersionSubtokenType.Unknown)
            {
                if (int.TryParse(_rawToken, out int numericValue))
                {
                    _numericValue = numericValue;
                    _tokenType = VersionSubtokenType.Numeric;
                }
                else
                {
                    _stringValue = _rawToken;
                    _tokenType = VersionSubtokenType.String;
                }
            }
        }

        public override string ToString()
        {
            return _rawToken;
        }

        public VersionSubtokenType TokenType
        {
            get
            {
                EnsureParsed();
                return _tokenType;
            }
        }

        public double NumericValue
        {
            get
            {
                EnsureParsed();
                if (TokenType != VersionSubtokenType.Numeric)
                {
                    return default(double);
                }
                return _numericValue;
            }
        }

        public string StringValue
        {
            get
            {
                EnsureParsed();
                if (TokenType != VersionSubtokenType.String)
                {
                    return string.Empty;
                }
                return _stringValue;
            }
        }

        public int CompareTo(MixedTypeComparisonToken otherToken)
        {
            return Compare(this, otherToken);
        }

        public static int Compare(MixedTypeComparisonToken token1, MixedTypeComparisonToken token2)
        {
            if (token1.TokenType != token2.TokenType)
            {   // the one with numeric info is higher
                return token1.TokenType == VersionSubtokenType.Numeric ? 1 : -1;
            }

            if (token1.TokenType == VersionSubtokenType.Numeric)
            {
                return token1.NumericValue.CompareTo(token2.NumericValue);
            }

            return token1.StringValue.CompareTo(token2.StringValue);
        }
    }
}
