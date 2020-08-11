﻿using AutoMapper;
using Bogus;
using poc_automapper_extensionmethod.Case01.Entities;
using poc_automapper_extensionmethod.Case01.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace poc_automapper_extensionmethod.Case01
{
    public static class Run
    {
        public static void Start()
        {
            Console.WriteLine("CASE 01");

            RunCases.WithMapper<UserModel, User>(InitializeAutomapper(), GenerateMock().First(), 1);
            RunCases.WithMapper<List<UserModel>, List<User>>(InitializeAutomapper(), GenerateMock(1_000), 1_000);
            RunCases.WithMapper<List<UserModel>, List<User>>(InitializeAutomapper(), GenerateMock(10_000), 10_000);
            RunCases.WithMapper<List<UserModel>, List<User>>(InitializeAutomapper(), GenerateMock(100_000), 100_000);
            RunCases.WithMapper<List<UserModel>, List<User>>(InitializeAutomapper(), GenerateMock(1_000_000), 1_000_000);

            WithoutMapper(GenerateMock(1));
            WithoutMapper(GenerateMock(1_000));
            WithoutMapper(GenerateMock(10_000));
            WithoutMapper(GenerateMock(100_000));
            WithoutMapper(GenerateMock(1_000_000));

            WithoutMapperAndWithoutLambda(GenerateMock(1_000));
            WithoutMapperAndWithoutLambda(GenerateMock(10_000));
            WithoutMapperAndWithoutLambda(GenerateMock(100_000));
            WithoutMapperAndWithoutLambda(GenerateMock(1_000_000));
        }

        private static void WithoutMapper(List<UserModel> source)
        {
            if (source.Count == 1)
                RunCases.Execute(() => source.First().Mapper(), "Without AutoMapper", source.Count);
            else
                RunCases.Execute(() => source.Mapper(), "Without AutoMapper", source.Count);
        }

        private static void WithoutMapperAndWithoutLambda(List<UserModel> source)
        {
            if (source.Count == 1)
                RunCases.Execute(() => source.First().Mapper(), "Without AutoMapper and without lambda", source.Count);
            else
                RunCases.Execute(() => source.MapperWithoutLambda(), "Without AutoMapper and without lambda", source.Count);
        }

        private static Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserModel, User>();
            });

            var mapper = new Mapper(config);
            return mapper;
        }

        private static List<UserModel> GenerateMock(int count = 1)
        {
            var user = new Faker<UserModel>()
                .RuleFor(r => r.Id, f => f.Random.Int())
                .RuleFor(r => r.Name, f => f.Name.FullName())
                .RuleFor(r => r.BirthDate, f => f.Date.Past())
                .RuleFor(r => r.Score, f => f.Random.Double())
                .Generate(count);

            return user;
        }
    }
}
