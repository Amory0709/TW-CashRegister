namespace CashRegisterTest
{
	using CashRegister;
    using Moq;
    using Xunit;

	public class CashRegisterTest
	{
		[Fact]
		public void Should_process_execute_printing()
		{
			//given
			var printer = new SpyPrinter();
			var cashRegister = new CashRegister(printer);
			var purchase = new Purchase();
			//when
			cashRegister.Process(purchase);
			//then
			//verify that cashRegister.process will trigger print
			Assert.True(printer.HasPrinted);
		}

		[Fact]
		public void Should_print_when_using_spy_printer()
        {
			//given
			var spyPrinter = new Mock<Printer>();
			var cashRegister = new CashRegister(spyPrinter.Object);
			var purchase = new Purchase();
			//when
			cashRegister.Process(purchase);
			//then
			//verify that cashRegister.process will trigger print
			spyPrinter.Verify(_ => _.Print(It.IsAny<string>()));
		}

		[Fact]
		public void Should_print_when_using_stub_purchase()
        {
			//given
			var spyPrinter = new Mock<Printer>();
			var cashRegister = new CashRegister(spyPrinter.Object);
			var mockPurchase = new Mock<Purchase>();
			mockPurchase.Setup(_ => _.AsString()).Returns("stub content");
			//when
			cashRegister.Process(mockPurchase.Object);
			//then
			//verify that cashRegister.process will trigger print
			spyPrinter.Verify(_ => _.Print("stub content"));
		}

		[Fact]
		public void Should_print_when_using_stub_purchase_throw_exception()
		{
			//given
			var spyPrinter = new Mock<Printer>();
			var cashRegister = new CashRegister(spyPrinter.Object);
			var mockPurchase = new Mock<Purchase>();
			mockPurchase.Setup(_ => _.AsString()).Throws(new PrinterOutOfPaperException());
			//when

			//then
			//verify that cashRegister.process will trigger print
			Assert.Throws<HardwareException>(() => cashRegister.Process(mockPurchase.Object));
		}
	}
}
