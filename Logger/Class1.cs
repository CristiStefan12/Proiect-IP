using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace Logger
{
    public class OllamaLoggerDecorator<T>
    {
        private Func<Task<T>> _func;

        public OllamaLoggerDecorator(Func<Task<T>> func)
        {
            _func = func;
        }

        public async Task<T> Run()
        {
            Console.WriteLine($"{DateTime.Now} - Started to run function {_func}");

            var output = await _func();

            Console.WriteLine($"{DateTime.Now} - Finished running function {_func}");

            return output;
        }
    }
}
