using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFEffects.Core.Common
{
    public delegate void DelegateMethod<T>(T arg);

    public delegate void DelegateMethod<T1, T2>(T1 arg1, T2 arg2);

    public delegate void DelegateMethod<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);
}
