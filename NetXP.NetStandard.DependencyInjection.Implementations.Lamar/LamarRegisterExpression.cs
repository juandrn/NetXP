﻿using Lamar;
using Lamar.IoC.Instances;
using NetXP.NetStandard.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetXP.NetStandard.DependencyInjection.Implementations.LamarDI
{
    public class LamarRegisterExpression : IRegister
    {
        private ServiceRegistry configuration;

        public LamarRegisterExpression(ServiceRegistry configuration)
        {
            this.configuration = configuration;
        }

        public void Register<TInterface, TImplement>()
            where TInterface : class
            where TImplement : class, TInterface
        {
            this.configuration.For<TInterface>().Use<TImplement>();
        }

        public void Register<TInterface, TImplement>(DILifeTime lifeTime)
            where TInterface : class
            where TImplement : class, TInterface
        {
            var register = this.configuration.For<TInterface>().Use<TImplement>();
            SetLifeTime(lifeTime, register);
        }

        public void Register<TInterface, TImplement>(string name) where TInterface : class
            where TImplement : class, TInterface
        {
            this.configuration.For<TInterface>().Use<TImplement>().Named(name);
        }

        public void Register<TInterface, TImplement>(string name, DILifeTime lifeTime) where TInterface : class
            where TImplement : class, TInterface
        {
            var register = this.configuration.For<TInterface>().Use<TImplement>().Named(name);
            SetLifeTime(lifeTime, register);
        }



        public void Register<TInterface, TImplement>(DILifeTime lifeTime,
                                                     Action<ICtorSelectorExpression<TImplement, TInterface>> ctorInjectorExpression)
                                                      where TInterface : class
            where TImplement : class, TInterface
        {
            var @for = this.configuration.For<TInterface>();
            var use = @for.Use<TImplement>();

            var ctorSelectorExpression = new LamarSelectorExpression<TImplement, TInterface>();
            ctorSelectorExpression.Register(@for, use, null, lifeTime, SetLifeTime);//Use, lifeTime, setLifeTime and Name only used for Empty constructors
            ctorInjectorExpression(ctorSelectorExpression);

            SetLifeTime(lifeTime, use);
        }

        public void Register<TInterface, TImplement>(string name,
                                                     DILifeTime lifeTime,
                                                     Action<ICtorSelectorExpression<TImplement, TInterface>> ctorInjectorExpression)
            where TInterface : class
            where TImplement : class, TInterface
        {
            var @for = this.configuration.For<TInterface>();
            var use = @for.Use<TImplement>();

            var ctorSelectorExpression = new LamarSelectorExpression<TImplement, TInterface>();
            ctorSelectorExpression.Register(@for, use, name, lifeTime, SetLifeTime);//Use, lifeTime, setLifeTime and Name only used for Empty constructors
            ctorInjectorExpression(ctorSelectorExpression);

            SetLifeTime(lifeTime, use);
        }
        public void RegisterInstance<TInterface>(TInterface instance)
            where TInterface : class
        {
            var register = this.configuration.For<TInterface>().Use(instance);
        }

        public void RegisterInstance<TInterface>(TInterface instance, DILifeTime lifeTime) 
            where TInterface : class
        {
            var register = this.configuration.For<TInterface>().Use(instance);
            SetLifeTime(lifeTime, register);
        }

        public void RegisterInstance<TInterface>(string name, TInterface instance, DILifeTime lifeTime)
            where TInterface : class
        {
            var register = this.configuration.For<TInterface>().Use(instance).Named(name);
            SetLifeTime(lifeTime, register);
        }

        private static void SetLifeTime<TInterface>(DILifeTime lifeTime, ConstructorInstance<TInterface> register)
        {
            if (lifeTime == DILifeTime.Scoped)
            {
                register.Scoped();
            }
            else if (lifeTime == DILifeTime.Singleton)
            {
                register.Singleton();
            }
            else if (lifeTime == DILifeTime.Trasient)
            {
                register.Transient();
            }
        }

        private static void SetLifeTime(DILifeTime lifeTime, ObjectInstance register)
        {
            if (lifeTime == DILifeTime.Scoped)
            {
                register.Scoped();
            }
            else if (lifeTime == DILifeTime.Singleton)
            {
                register.Singleton();
            }
            else if (lifeTime == DILifeTime.Trasient)
            {
                register.Transient();
            }
        }
    }
}
