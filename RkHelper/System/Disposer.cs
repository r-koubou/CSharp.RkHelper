using System;

namespace RkHelper.System
{
    public static class Disposer
    {
        public static void Dispose( IDisposable disposable )
        {
            try { disposable.Dispose(); } catch { /* ignored */ }
        }

        public static void Dispose( params IDisposable[] disposables )
        {
            foreach( var x in disposables )
            {
                try { x.Dispose(); } catch { /* ignored */ }
            }
        }
    }
}