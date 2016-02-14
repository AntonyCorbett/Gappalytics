﻿namespace Appalytics.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class CustomVariableBag
    {
        internal readonly KeyValuePair<string, string>?[] _variables;

        public CustomVariableBag()
           : this(new KeyValuePair<string, string>?[5])
        {
        }

        public CustomVariableBag(KeyValuePair<string, string>?[] variables)
        {
            _variables = variables;
        }

        public void Set(int position, string key, string value)
        {
            if (position < 1 || position > 5)
                throw new ArgumentOutOfRangeException("position");

            _variables[position - 1] = new KeyValuePair<string, string>(key, value);
        }

        public CustomVariableBag MergeWith(CustomVariableBag other)
        {
            var variables = new KeyValuePair<string, string>?[5];

            _variables.CopyTo(variables, 0);

            for (int i = 0; i < 5; i++)
            {
                if (other._variables[i] != null)
                {
                    variables[i] = other._variables[i];
                }
            }

            return new CustomVariableBag(variables);
        }

        public bool Any()
        {
            return _variables.Any(v => v != null);
        }

        public string ToUtme()
        {
            return "8(" +
                   string.Join("*", _variables.Where(v => v != null).Select(kvp => AnalyticsClient.EncodeUtmePart(kvp.Value.Key)).ToArray()) +
                   ")9(" +
                   string.Join("*", _variables.Where(v => v != null).Select(kvp => AnalyticsClient.EncodeUtmePart(kvp.Value.Value)).ToArray()) +
                   ")11(" +
                   string.Join("*", _variables.Where(v => v != null).Select(kvp => "1").ToArray());
        }

        public void Clear(int position)
        {
            _variables[position - 1] = null;
        }
    }
}
