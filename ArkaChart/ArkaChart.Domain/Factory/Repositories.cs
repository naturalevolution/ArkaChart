using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArkaChart.Domain.Mapping.Context;
using ArkaChart.Domain.Repositories;
using ArkaChart.Domain.Repositories.Impl;

namespace ArkaChart.Domain.Factory {
    public class Repositories : IRepositories {
        private static IRepositories soleInstance;
        private static IDbContext soleInstanceContext;

        public static IFileRepository Files {
            get { return SoleInstance.GetFileRepository(); }
        }
        public static IFileLineRepository FileLines {
            get { return SoleInstance.GetFileLineRepository(); }
        }
        public IFileRepository GetFileRepository() {
            return new FileRepository(SoleInstanceContext);
        }
        public IFileLineRepository GetFileLineRepository() {
            return new FileLineRepository(SoleInstanceContext);
        }

        private static IRepositories SoleInstance {
            get {
                if (soleInstance == null) {
                    throw new InvalidOperationException("Repositories must be loaded");
                }
                return soleInstance;
            }
        }

        private IDbContext SoleInstanceContext {
            get {
                if (soleInstanceContext == null) {
                    throw new InvalidOperationException("Repository's Context must be loaded");
                }
                return soleInstanceContext;
            }
        }

        public static void Load(IRepositories repositoryFactory) {
            soleInstance = repositoryFactory;
        }

        public static void LoadContext(IDbContext context) {
            soleInstanceContext = context;
        }

        public static void SaveChanges() {
            soleInstanceContext.GetContext().SaveChanges();
        }

    }

    public interface IRepositories {
        IFileRepository GetFileRepository();
        IFileLineRepository GetFileLineRepository();
    }
}
