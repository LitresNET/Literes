using AutoFixture;
using Litres.Data.Abstractions.Repositories;
using Litres.Data.Models;
using Litres.Main.Services;
using Moq;
using Tests.Config;

namespace Tests.MainService.Services.BookServiceTests;

public class GetBookCatalog
{
    private readonly Mock<IAuthorRepository> _authorRepositoryMock = new();
    private readonly Mock<IBookRepository> _bookRepositoryMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<ISeriesRepository> _seriesRepositoryMock = new();
    private readonly Mock<IPublisherRepository> _publisherRepositoryMock = new();
    private readonly Mock<IRequestRepository> _requestRepositoryMock = new();

    private BookService BookService => new(
        _authorRepositoryMock.Object,
        _userRepositoryMock.Object,
        _bookRepositoryMock.Object,
        _seriesRepositoryMock.Object,
        _publisherRepositoryMock.Object,
        _requestRepositoryMock.Object
        );
    
    public static IEnumerable<object[]> TestData =>
        new List<object[]>
        {
            new object[]
            {
                new Dictionary<SearchParameter, string>
                {
                    { SearchParameter.Category, "Fantasy" },
                    { SearchParameter.New, "true" },
                    { SearchParameter.HighRated, "false" }
                },
                1,
                10
            }, new object[]
            {
                new Dictionary<SearchParameter, string>
                {
                    { SearchParameter.Category, "Action" },
                    { SearchParameter.New, "false" },
                    { SearchParameter.HighRated, "true" }
                },
                2,
                10
            }, new object[]
            {
                new Dictionary<SearchParameter, string>
                {
                    { SearchParameter.Category, "Horror" },
                    { SearchParameter.New, "false" },
                    { SearchParameter.HighRated, "false" }
                },
                3,
                10
            }, new object[]
            {
                new Dictionary<SearchParameter, string>
                {
                    { SearchParameter.Category, "Fairytale" },
                    { SearchParameter.New, "false" },
                    { SearchParameter.HighRated, "true" }
                },
                4,
                10
            }
        };

    [Theory]
    [MemberData(nameof(TestData))]
    public async Task DefaultRequests_ReturnBookLists(
        Dictionary<SearchParameter, string> searchParameters, 
        int extraLoadNumber, 
        int booksAmount)
    {
        // Arrange
        var rnd = new Random();
        var allGenres = Enum.GetValues(typeof(Genre)).Cast<Genre>().ToList();
        var fixture = new Fixture().Customize(new AutoFixtureCustomization());

        var books = new List<Book>();
        for (var i = 0; i < 35; i++)
        {
            var chosenGenres = GetRandomElementsDistinct(new List<Genre>(allGenres), rnd.Next(0, allGenres.Count));
            books.Add(fixture
                .Build<Book>()
                .With(b => b.Id, i)
                .With(b => b.Rating, rnd.Next(0, 5))
                .With(b => b.BookGenres, chosenGenres)
                .Create()
            );
        }
        
        var value = searchParameters[SearchParameter.Category];
        var genre = Enum.Parse<Genre>(value);
        Func<Book, bool> predicate = b => b.BookGenres.Contains(genre);
        var filteredBooks = books.Where(b => predicate(b)).AsQueryable();
        
        _bookRepositoryMock
            .Setup(r => r.GetBooksByFilterAsync(It.IsAny<Func<Book,bool>>()))
            .ReturnsAsync(filteredBooks);
        
        var orderedBooks = bool.Parse(searchParameters[SearchParameter.New])
            ? filteredBooks.OrderByDescending(b => b.PublicationDate)
            : filteredBooks.OrderBy(b => b.PublicationDate);

        orderedBooks = bool.Parse(searchParameters[SearchParameter.HighRated])
            ? orderedBooks.ThenByDescending(b => b.Rating)
            : orderedBooks.ThenBy(b => b.Rating);
        
        var expected = orderedBooks
            .Skip((extraLoadNumber - 1) * booksAmount)
            .Take(booksAmount)
            .ToList();
        
        // Act 
        var actual = await BookService.GetBookCatalogAsync(searchParameters, extraLoadNumber, booksAmount);
        
        // Assert
        Assert.Equal(expected.Count, actual.Count);
        for (var i = 0; i < expected.Count; i++)
            Assert.Equal(expected[i].BookGenres, actual[i].BookGenres);
        
    }
    
    private static List<T> GetRandomElementsDistinct<T>(IList<T> list, int numberOfElements)
    {
        var rnd = new Random();
        var randomElements = new List<T>();

        for (var i = 0; i < numberOfElements; i++)
        {
            var index = rnd.Next(list.Count);
            randomElements.Add(list[index]);
            list.RemoveAt(index);
        }

        return randomElements;
    }
}