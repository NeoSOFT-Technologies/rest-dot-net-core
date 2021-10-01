using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Application.Features.Categories.Commands.CreateCateogry;
using GloboTicket.TicketManagement.Application.Profiles;
using GloboTicket.TicketManagement.Application.Responses;
using GloboTicket.TicketManagement.Application.UnitTests.Mocks;
using Moq;
using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GloboTicket.TicketManagement.Application.UnitTests.Categories.Commands
{
    public class CreateCategoryTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly Mock<IMessageRepository> _mockMessageRepository;

        public CreateCategoryTests()
        {
            _mockCategoryRepository = CategoryRepositoryMocks.GetCategoryRepository();
            _mockMessageRepository = MessageRepositoryMocks.GetMessageRepository();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Handle_ValidCategory_AddedToCategoryRepository()
        {
            var handler = new CreateCategoryCommandHandler(_mapper, _mockCategoryRepository.Object, _mockMessageRepository.Object);

            var result = await handler.Handle(new CreateCategoryCommand() { Name = "Test" }, CancellationToken.None);

            var allCategories = await _mockCategoryRepository.Object.ListAllAsync();

            result.ShouldBeOfType<Response<CreateCategoryDto>>();
            result.Succeeded.ShouldBe(true);
            result.Errors.ShouldBeNull();
            allCategories.Count.ShouldBe(5);
        }

        [Fact]
        public async Task Handle_EmptyCategory_AddedToCategoryRepository()
        {
            var handler = new CreateCategoryCommandHandler(_mapper, _mockCategoryRepository.Object, _mockMessageRepository.Object);

            var result = await handler.Handle(new CreateCategoryCommand() { Name = "" }, CancellationToken.None);

            var allCategories = await _mockCategoryRepository.Object.ListAllAsync();

            result.ShouldBeOfType<Response<CreateCategoryDto>>();
            result.Succeeded.ShouldBe(false);
            result.Errors.ShouldNotBeNull();
            result.Errors[0].ToLower().ShouldBe("name is required.");
            allCategories.Count.ShouldBe(4);
        }

        [Fact]
        public async Task Handle_CategoryLength_GreaterThan_10_AddedToCategoryRepository()
        {
            var handler = new CreateCategoryCommandHandler(_mapper, _mockCategoryRepository.Object, _mockMessageRepository.Object);

            var result = await handler.Handle(new CreateCategoryCommand() { Name = "TEST123456780" }, CancellationToken.None);

            var allCategories = await _mockCategoryRepository.Object.ListAllAsync();

            result.ShouldBeOfType<Response<CreateCategoryDto>>();
            result.Succeeded.ShouldBe(false);
            result.Errors.ShouldNotBeNull();
            result.Errors[0].ToLower().ShouldBe("name must not exceed 10 characters.");
            allCategories.Count.ShouldBe(4);
        }
    }
}
