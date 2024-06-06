using Microsoft.EntityFrameworkCore;
using Moq;

namespace TestProject1
{
    public static class DbSetMock
    {
        public static Mock<DbSet<T>> GetDbSetMock<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();
            var dbSetMock = new Mock<DbSet<T>>();

            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            dbSetMock.Setup(d => d.Add(It.IsAny<T>())).Callback<T>(sourceList.Add);
            dbSetMock.Setup(d => d.Remove(It.IsAny<T>())).Callback<T>(t => sourceList.Remove(t));
            dbSetMock.Setup(d => d.AddAsync(It.IsAny<T>(), It.IsAny<CancellationToken>())).Callback((T model, CancellationToken token) => sourceList.Add(model));

            return dbSetMock;
        }

    }
}
