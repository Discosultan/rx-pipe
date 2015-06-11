﻿using System;
using System.Reactive.Linq;
using RxPipe.Lib.Processors;
using RxPipe.Lib.Providers;
using RxPipe.Lib.Utilities;

namespace RxPipe.Lib
{
    public class ReactivePipe<T>
    {
        private readonly IPipeProvider<T> _provider;
        private readonly IPipeProcessor<T>[] _processors;

        public ReactivePipe(IPipeProvider<T> provider, params IPipeProcessor<T>[] processors)
        {
            Guard.NotNull(provider, nameof(provider));
            Guard.NotNull(processors, nameof(processors));
            _provider = provider;
            _processors = processors;
        }

        public IObservable<T> Process()
        {
            //return _provider.GetAll().Do(async item =>
            //{
            //    foreach (IPipeProcessor<T> processor in _processors)
            //    {
            //        item = await processor.ProcessAsync(item);
            //    }
            //});

            return Observable.Create<T>(async observer =>
            {
                await _provider.GetAll().Do(async item =>
                {
                    foreach (IPipeProcessor<T> processor in _processors)
                    {
                        item = await processor.ProcessAsync(item);
                    }
                    observer.OnNext(item);
                });
            });

            //return _provider.GetAll().Select<T,T>(async article =>
            //{
            //    Task<T> articleTask;
            //    foreach (IPipeProcessor<T> processor in _processors)
            //    {
            //        articleTask = articleTask.ContinueWith(async article => processor.ProcessAsync(article);
            //    }
            //    return article;                
            //});
        }
    }
}