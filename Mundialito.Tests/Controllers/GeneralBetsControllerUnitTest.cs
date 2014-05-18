﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Mundialito.Controllers;
using Mundialito.DAL.GeneralBets;
using Mundialito.DAL.Teams;
using Mundialito.Logic;
using Mundialito.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundialito.Tests.Controllers
{
    [TestClass]
    public class GeneralBetsControllerUnitTest
    {
        [TestMethod]
        public void GetAllGeneralBetsTest()
        {
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            var repository = new Mock<IGeneralBetsRepository>();
            var userProvider = new Mock<ILoggedUserProvider>();
            var bets = new List<GeneralBet>();
            bets.Add(new GeneralBet() { GeneralBetId = 1, WinningTeam = new Team() { TeamId = 1, Name = "A" }});
            bets.Add(new GeneralBet() { GeneralBetId = 2, WinningTeam = new Team() { TeamId = 1, Name = "A" } });
            repository.Setup(rep => rep.GetGeneralBets()).Returns(bets);

            var controller = new GeneralBetsController(repository.Object, userProvider.Object, dateTimeProvider.Object);
            var res = controller.GetAllGeneralBets();
            Assert.AreEqual(2, res.Count());
        }

        [TestMethod]
        public void GetGeneralBetsByIdTest()
        {
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            var repository = new Mock<IGeneralBetsRepository>();
            var userProvider = new Mock<ILoggedUserProvider>();
            var bets = new List<GeneralBet>();
            bets.Add(new GeneralBet() { GeneralBetId = 1, WinningTeam = new Team() { TeamId = 1, Name = "A" } });
            bets.Add(new GeneralBet() { GeneralBetId = 2, WinningTeam = new Team() { TeamId = 1, Name = "A" } });
            repository.Setup(rep => rep.GetGeneralBet(1)).Returns(bets[0]);
            repository.Setup(rep => rep.GetGeneralBet(2)).Returns(bets[1]);

            var controller = new GeneralBetsController(repository.Object, userProvider.Object, dateTimeProvider.Object);
            var res = controller.GetGeneralBetById(1);
            Assert.AreEqual(1, res.GeneralBetId);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFoundException))]
        public void GetGeneralBetsByNonExistIdTest()
        {
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            var repository = new Mock<IGeneralBetsRepository>();
            var userProvider = new Mock<ILoggedUserProvider>();
            var bets = new List<GeneralBet>();
            repository.Setup(rep => rep.GetGeneralBet(1)).Returns((GeneralBet) null);

            var controller = new GeneralBetsController(repository.Object, userProvider.Object, dateTimeProvider.Object);
            var res = controller.GetGeneralBetById(1);
        }

        [TestMethod]
        public void GetUserGeneralBetsTest()
        {
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            var repository = new Mock<IGeneralBetsRepository>();
            var userProvider = new Mock<ILoggedUserProvider>();
            var bets = new List<GeneralBet>();
            bets.Add(new GeneralBet() { GeneralBetId = 1, WinningTeam = new Team() { TeamId = 1, Name = "A" } });
            bets.Add(new GeneralBet() { GeneralBetId = 2, WinningTeam = new Team() { TeamId = 1, Name = "A" } });
            repository.Setup(rep => rep.GetUserGeneralBet("ezraroi")).Returns(new GeneralBet() { GeneralBetId = 1, WinningTeam = new Team() { TeamId = 1, Name = "A" } });

            var controller = new GeneralBetsController(repository.Object, userProvider.Object, dateTimeProvider.Object);
            var res = controller.GetUserGeneralBet("ezraroi");
            Assert.AreEqual(1, res.GeneralBetId);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFoundException))]
        public void GetNonExsitingUserGeneralBetsTest()
        {
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            var repository = new Mock<IGeneralBetsRepository>();
            var userProvider = new Mock<ILoggedUserProvider>();
            var bets = new List<GeneralBet>();
            bets.Add(new GeneralBet() { GeneralBetId = 1, WinningTeam = new Team() { TeamId = 1, Name = "A" } });
            bets.Add(new GeneralBet() { GeneralBetId = 2, WinningTeam = new Team() { TeamId = 1, Name = "A" } });
            repository.Setup(rep => rep.GetUserGeneralBet("ezraroi")).Returns( (GeneralBet)null);

            var controller = new GeneralBetsController(repository.Object, userProvider.Object, dateTimeProvider.Object);
            var res = controller.GetUserGeneralBet("ezraroi");
        }

        [TestMethod]
        public void GetUserHasGeneralBetsTest()
        {
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            var repository = new Mock<IGeneralBetsRepository>();
            var userProvider = new Mock<ILoggedUserProvider>();
            var bets = new List<GeneralBet>();
            repository.Setup(rep => rep.IsGeneralBetExists("ezraroi")).Returns(false);

            var controller = new GeneralBetsController(repository.Object, userProvider.Object, dateTimeProvider.Object);
            Assert.IsFalse(controller.HasBet("ezraroi"));
        }

        [TestMethod]
        public void ResolveGeneralBetsTest()
        {
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            var repository = new Mock<IGeneralBetsRepository>();
            var userProvider = new Mock<ILoggedUserProvider>();
            var generalBet = new GeneralBet() { GeneralBetId = 1, WinningTeam = new Team() { TeamId = 1, Name = "A" } };
            repository.Setup(rep => rep.GetGeneralBet(1)).Returns(generalBet);
            dateTimeProvider.Setup(item => item.UTCNow).Returns(new DateTime(2014, 8, 1));

            Assert.IsFalse(generalBet.IsResolved);
            var controller = new GeneralBetsController(repository.Object, userProvider.Object, dateTimeProvider.Object);
            controller.ResolveGeneralBet(1, new ResolveGeneralBetModel() { TeamIsRight = true, PlayerIsRight = true });
            Assert.AreEqual(12, generalBet.PlayerPoints);
            Assert.AreEqual(12, generalBet.TeamPoints);
            Assert.IsTrue(generalBet.IsResolved);
            repository.Verify(item => item.Save());
        }

        [TestMethod]
        public void ResolveGeneralBets2Test()
        {
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            var repository = new Mock<IGeneralBetsRepository>();
            var userProvider = new Mock<ILoggedUserProvider>();
            var generalBet = new GeneralBet() { GeneralBetId = 1, WinningTeam = new Team() { TeamId = 1, Name = "A" } };
            repository.Setup(rep => rep.GetGeneralBet(1)).Returns(generalBet);
            dateTimeProvider.Setup(item => item.UTCNow).Returns(new DateTime(2014, 8, 1));

            Assert.IsFalse(generalBet.IsResolved);
            var controller = new GeneralBetsController(repository.Object, userProvider.Object, dateTimeProvider.Object);
            controller.ResolveGeneralBet(1, new ResolveGeneralBetModel() { TeamIsRight = false, PlayerIsRight = true });
            Assert.AreEqual(12, generalBet.PlayerPoints);
            Assert.AreEqual(0, generalBet.TeamPoints);
            Assert.IsTrue(generalBet.IsResolved);
            repository.Verify(item => item.Save());
        }

        [TestMethod]
        public void ResolveGeneralBets3Test()
        {
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            var repository = new Mock<IGeneralBetsRepository>();
            var userProvider = new Mock<ILoggedUserProvider>();
            var generalBet = new GeneralBet() { GeneralBetId = 1, WinningTeam = new Team() { TeamId = 1, Name = "A" } };
            repository.Setup(rep => rep.GetGeneralBet(1)).Returns(generalBet);
            dateTimeProvider.Setup(item => item.UTCNow).Returns(new DateTime(2014, 8, 1));

            Assert.IsFalse(generalBet.IsResolved);
            var controller = new GeneralBetsController(repository.Object, userProvider.Object, dateTimeProvider.Object);
            controller.ResolveGeneralBet(1, new ResolveGeneralBetModel() { TeamIsRight = false, PlayerIsRight = false });
            Assert.AreEqual(0, generalBet.PlayerPoints);
            Assert.AreEqual(0, generalBet.TeamPoints);
            Assert.IsTrue(generalBet.IsResolved);
            repository.Verify(item => item.Save());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ResolveNotClosedGeneralBetsTest()
        {
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            var repository = new Mock<IGeneralBetsRepository>();
            var userProvider = new Mock<ILoggedUserProvider>();
            var generalBet = new GeneralBet() { GeneralBetId = 1, WinningTeam = new Team() { TeamId = 1, Name = "A" } };
            repository.Setup(rep => rep.GetGeneralBet(1)).Returns(generalBet);
            dateTimeProvider.Setup(item => item.UTCNow).Returns(new DateTime(2014, 6, 1));

            Assert.IsFalse(generalBet.IsResolved);
            var controller = new GeneralBetsController(repository.Object, userProvider.Object, dateTimeProvider.Object);
            controller.ResolveGeneralBet(1, new ResolveGeneralBetModel() { TeamIsRight = false, PlayerIsRight = false });
        }
    }
}
