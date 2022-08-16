using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class HouseKeeperServiceTests
    {
        private HouseKeeperService _service;
        private Mock<IEmailSender> _emailSender;
        private Mock<IStatementGenerator> _statementGenerator;
        private Mock<IXtraMessageBox> _messageBox;
        private DateTime _statementDate = new DateTime(2017, 1, 1);
        private Housekeeper _housekeeper;
        private string _filename = "fileName";
        [SetUp]
        public void SetUp()
        {
            var unitToWork = new Mock<IUnitOfWork>();
            _housekeeper = new Housekeeper
            {
                Email = "a",
                FullName = "b",
                Oid = 1,
                StatementEmailBody = "c"
            };
            unitToWork.Setup(x => x.Query<Housekeeper>()).Returns(new List<Housekeeper>
            {
                _housekeeper
            }.AsQueryable());
            _filename = "fileName";
            _statementGenerator = new Mock<IStatementGenerator>();
            _statementGenerator.Setup(sg => sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate)).Returns(() => _filename);
            _emailSender = new Mock<IEmailSender>();
            _messageBox = new Mock<IXtraMessageBox>();
            _service = new HouseKeeperService(unitToWork.Object, _statementGenerator.Object, _emailSender.Object, _messageBox.Object);

        }
        [Test]
        public void SendStatementEmails_WhenCalled_GenerateStatements()
        {

            _service.SendStatementEmails(_statementDate);
            _statementGenerator.Verify(sg => sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate));
        }
        [Test]
        [TestCase(null)]
        [TestCase(" ")]
        [TestCase("")]
        public void SendStatementEmails_HouseKeepersEmailIsNullEmptyOrWhiteSpace_ShouldNotGenerateStatement(string email)
        {
            _housekeeper.Email = email;
            _service.SendStatementEmails(_statementDate);
            _statementGenerator.Verify(sg => sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate),Times.Never);
        }
        [Test]
        public void SendStatementEmails_WhenCalled_EmailTheStatement()
        {
            _statementGenerator.Setup(sg => sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate)).Returns(_filename);
            _service.SendStatementEmails(_statementDate);

            _emailSender.Verify(es => es.EmailFile(
                _housekeeper.Email,
                _housekeeper.StatementEmailBody,
                _filename,
                It.IsAny<string>()));
        }
        [Test]
        public void SendStatementEmails_StatementFileNameIsNull_ShouldNotEmailTheStatement()
        {
            _filename = "";

            _service.SendStatementEmails(_statementDate);

            _emailSender.Verify(es => es.EmailFile(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()),Times.Never);
        }
        [Test]
        public void SendStatementEmails_StatementFileNameIsEmptyString_ShouldNotEmailTheStatement()
        {
            _filename = "";
            _service.SendStatementEmails(_statementDate);

            _emailSender.Verify(es => es.EmailFile(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()), Times.Never);
        }
        [Test]
        public void SendStatementEmails_StatementFileNameIsWhiteSpace_ShouldNotEmailTheStatement()
        {
            _filename = " ";

            _service.SendStatementEmails(_statementDate);

            _emailSender.Verify(es => es.EmailFile(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()), Times.Never);
        }
        [Test]
        public void SendStatementEmails_EmailSendingFails_DisplayAMessageBox()
        {
            _filename = "file";
            _emailSender.Setup(es => es.EmailFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Throws<Exception>();
            _service.SendStatementEmails(_statementDate);

            _messageBox.Verify(mb => mb.Show(It.IsAny<string>(), It.IsAny<string>(), MessageBoxButtons.OK));
        }
    }
}
