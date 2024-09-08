using Api.Domain.Entities;

namespace Api.Application.Interfaces
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IRepository<Order> orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task CreateOrderAsync(Order order)
        {
            await _orderRepository.AddAsync(order);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _orderRepository.UpdateAsync(order);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            _orderRepository.DeleteAsync(order);
            await _unitOfWork.CompleteAsync();
        }
    }
}
