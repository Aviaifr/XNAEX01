using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameInfrastructure.ObjectModel
{
    /// <summary>
    /// A comparer designed to assist with sorting IUpdateable interfaces.
    /// </summary>
    public sealed class UpdateableComparer : IComparer<IUpdateable>
    {
        /// <summary>
        /// A static copy of the comparer to avoid the GC.
        /// </summary>
        public static readonly UpdateableComparer Default;

        static UpdateableComparer()
        {
            Default = new UpdateableComparer();
        }

        private UpdateableComparer()
        {
        }

        public int Compare(IUpdateable x, IUpdateable y)
        {
            const int k_XBigger = 1;
            const int k_Equal = 0;
            const int k_YBigger = -1;

            int retCompareResult = k_YBigger;

            if (x == null && y == null)
            {
                retCompareResult = k_Equal;
            }
            else if (x != null)
            {
                if (y == null)
                {
                    retCompareResult = k_XBigger;
                }
                else if (x.Equals(y))
                {
                    return k_Equal;
                }
                else if (x.UpdateOrder > y.UpdateOrder)
                {
                    return k_XBigger;
                }
            }

            return retCompareResult;
        }
    }
}
