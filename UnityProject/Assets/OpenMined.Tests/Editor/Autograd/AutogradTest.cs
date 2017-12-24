using System;
using System.Linq;
using Nethereum.ABI.Decoders;
using NUnit.Framework;
using OpenMined.Network.Controllers;
using OpenMined.Syft.NN;
using UnityEngine;

namespace OpenMined.Tests.Editor.Autograd
{
    [Category("Autograd Tests")]
    public class AutogradTests
    {
        private SyftController ctrl;

        [OneTimeSetUp]
        public void Init()
        {
            //Init runs once before running test cases.
            ctrl = new SyftController(null);
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            //CleanUp runs once after all test cases are finished.
        }

        [SetUp]
        public void SetUp()
        {
            //SetUp runs before all test cases
        }

        [TearDown]
        public void TearDown()
        {
            //SetUp runs after all test cases
        }

        /********************/
        /* Tests Start Here */
        /********************/
        [Test]
        public void TanhAutograd()
        {
            float[] data1 = { 1, 2, 3, 4 };
            int[] shape1 = { 2, 2 };
            var tensor = ctrl.floatTensorFactory.Create(_data: data1, _shape: shape1, _autograd: true);

            float[] data2 = { 0.4200f, 0.0707f, 0.0099f, 0.0013f };
            int[] shape2 = { 2, 2 };

            var expectedGradTensor = ctrl.floatTensorFactory.Create(_data: data2, _shape: shape2);

            float[] data3 = { 0.7616f, 0.9640f, 0.9951f, 0.9993f };
            int[] shape3 = { 2, 2 };

            var expectedTanhTensor = ctrl.floatTensorFactory.Create(_data: data3, _shape: shape3);

            var tanhTensor = tensor.Tanh();

            for (var i = 0; i < tensor.Size; i++)
            {
                Assert.AreEqual(expectedTanhTensor.Data[i], tanhTensor.Data[i], 1e-4);
            }

            tanhTensor.Backward();

            for (var i = 0; i < tensor.Size; i++)
            {
                Assert.AreEqual(expectedGradTensor.Data[i], tensor.Grad.Data[i], 1e-4);
            }
        }

        [Test]
        public void SigmoidAutograd()
        {
            float[] data1 = { 1, 2, 3, 4 };
            int[] shape1 = { 2, 2 };
            var tensor1 = ctrl.floatTensorFactory.Create(_data: data1, _shape: shape1, _autograd: true);

            float[] data2 = { 0.1966f, 0.1050f, 0.0452f, 0.0177f };
            int[] shape2 = { 2, 2 };

            var expectedGradTensor = ctrl.floatTensorFactory.Create(_data: data2, _shape: shape2);

            var sigmoidTensor = tensor1.Sigmoid();

            sigmoidTensor.Backward();

            for (var i = 0; i < tensor1.Size; i++) {
                Assert.AreEqual(expectedGradTensor.Data[i], tensor1.Grad.Data[i], 1e-4);
            }
        }
    }
}
