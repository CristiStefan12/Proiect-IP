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
        /// <summary>
        /// The construcotr for OllamaLoggerDecorator
        /// </summary>
        /// <param name="func"></param>
        public OllamaLoggerDecorator(Func<Task<T>> func)
        {
            _func = func;
        }
        /// <summary>
        /// Executes the asynchronous function `_func` and logs the start and end times.
        /// </summary>
        /// <typeparam name="T">The return type of the asynchronous function.</typeparam>
        /// <returns>A task that represents the asynchronous operation, with a result of type <typeparamref name="T"/>.</returns>
        public async Task<T> Run()
        {
            Console.WriteLine($"{DateTime.Now} - Started to run function {_func}");

            var output = await _func();

            Console.WriteLine($"{DateTime.Now} - Finished running function {_func}");

            return output;
        }
    }
}
