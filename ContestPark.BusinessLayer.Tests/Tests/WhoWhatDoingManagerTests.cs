using ContestPark.BusinessLayer.Interfaces;
using ContestPark.BusinessLayer.Services;
using ContestPark.DataAccessLayer.Interfaces;
using ContestPark.DataAccessLayer.Tables;
using ContestPark.Entities.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace ContestPark.BusinessLayer.Tests.Tests
{
    [TestClass]
    public class PostManagerTests
    {
        #region Field

        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IPostRepository> _repository;
        private IPostService _PostService;

        #endregion Field

        #region Test settings method

        [TestInitialize]
        public void Initialize()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _repository = new Mock<IPostRepository>();
        }

        private void CreateManager()
        {
            _PostService = new PostService(_repository.Object, _unitOfWork.Object);
        }

        #endregion Test settings method

        #region Constructors methods tests

        /// <summary>
        /// Eğer IPostRepository null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IPostRepository_Null_Expect_ArgumentNullException()
        {
            // Act
            IPostService repository = new PostService(null, _unitOfWork.Object);
        }

        /// <summary>
        /// Eğer IUnitOfWork null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void When_IUnitOfWork_Null_Expect_ArgumentNullException()
        {
            // Act
            IPostService repository = new PostService(_repository.Object, null);
        }

        #endregion Constructors methods tests

        #region Update method tests

        /// <summary>
        /// Eğer Post null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Update_Post_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            Post nullPost = null;
            // Act
            _PostService.Update(nullPost);
        }

        /// <summary>
        /// Eğer GetById çağırınca null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Update_GetById_Result_Null_Expect_Exception()
        {
            // Arrange
            _unitOfWork.Setup(x => x.Repository<Post>()).Returns(_repository.Object);
            CreateManager();
            Post samplePost = new Post { };
            // Act
            _PostService.Update(samplePost);
        }

        #endregion Update method tests

        #region PostList method tests

        /// <summary>
        /// Eğer UserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PostList_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _PostService.PostList(nullUserId, "sampe-user-name", new PagingModel());
        }

        /// <summary>
        /// Eğer UserName null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PostList_UserName_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserName = String.Empty;
            // Act
            _PostService.PostList("sampe-user-id", nullUserName, new PagingModel());
        }

        #endregion PostList method tests

        #region Insert method tests

        /// <summary>
        /// Eğer Post null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Insert_Post_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            Post nullPost = null;
            // Act
            _PostService.Insert(nullPost);
        }

        #endregion Insert method tests

        #region Post method tests

        /// <summary>
        /// Eğer UserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Post_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _PostService.Post(nullUserId, 1);
        }

        /// <summary>
        /// Eğer PostId 0 gelirse InvalidOperationException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Post_PostId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int zeroPostId = 0;
            // Act
            _PostService.Post("sampe-user-id", zeroPostId);
        }

        #endregion Post method tests

        #region GetUserId method tests

        /// <summary>
        /// Eğer PostId 0 gelirse InvalidOperationException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetUserId_PostId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int zeroPostId = 0;
            // Act
            _PostService.GetUserId(zeroPostId);
        }

        #endregion GetUserId method tests

        #region IsFollowControl method tests

        /// <summary>
        /// Eğer userId 0 gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IsFollowControl_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _PostService.IsFollowControl(nullUserId, "sample-contestant-Id");
        }

        /// <summary>
        /// Eğer contestantId 0 gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IsFollowControl_ContestantId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullContestantId = String.Empty;
            // Act
            _PostService.IsFollowControl("sample-user-Id", nullContestantId);
        }

        #endregion IsFollowControl method tests

        #region ContestEnterScreen method tests

        /// <summary>
        /// Eğer UserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ContestEnterScreen_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _PostService.ContestEnterScreen(nullUserId, 1, new PagingModel());
        }

        /// <summary>
        /// Eğer SubCategoryId 0 gelirse InvalidOperationException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ContestEnterScreen_SubCategoryId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int zeroSubCategoryId = 0;
            // Act
            _PostService.ContestEnterScreen("sampe-user-id", zeroSubCategoryId, new PagingModel());
        }

        #endregion ContestEnterScreen method tests

        #region ContestEnterScreen method tests

        /// <summary>
        /// Eğer UserId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Update_UserId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullUserId = String.Empty;
            // Act
            _PostService.Update(nullUserId, "sample-contestant-id", 1);
        }

        /// <summary>
        /// Eğer ContestantId null gelirse ArgumentNullException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Update_ContestantId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            string nullContestantId = String.Empty;
            // Act
            _PostService.Update("sample-user-id", nullContestantId, 1);
        }

        /// <summary>
        /// Eğer PostId 0 gelirse InvalidOperationException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Update_SubCategoryId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int zeroDuelId = 0;
            // Act
            _PostService.Update("sampe-user-id", "sample-contestant-id", zeroDuelId);
        }

        #endregion ContestEnterScreen method tests

        #region DeleteAllPostByPictureId method tests

        /// <summary>
        /// Eğer pictuteId 0 gelirse InvalidOperationException fırlatması lazım
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DeleteAllPostByPictureId_PictuteId_Null_Expect_Exception()
        {
            // Arrange
            CreateManager();
            int zeroPictuteId = 0;
            // Act
            _PostService.DeleteAllPostByPictureId(zeroPictuteId);
        }

        /// <summary>
        /// Eğer GetPostsByPictureId methodu null gelirse InvalidOperationException fırlatması lazım
        /// </summary>
        [TestMethod]
        public void DeleteAllPostByPictureId_GetPostsByPictureId_Result_Null_Expect_Exception()
        {
            // Arrange
            _repository.Setup(x => x.GetPostsByPictureId(It.IsAny<int>())).Returns((IEnumerable<int>)null);
            CreateManager();
            // Act
            _PostService.DeleteAllPostByPictureId(122);
        }

        #endregion DeleteAllPostByPictureId method tests
    }
}