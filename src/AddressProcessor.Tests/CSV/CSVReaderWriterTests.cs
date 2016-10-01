using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AddressProcessing.CSV;
using NUnit.Framework;

namespace Csv.Tests
{
    [TestFixture]
    public class CSVReaderWriterTests
    {
        [Test]
        public void test_read_from_csv()
        {
            var reader = new CSVReaderWriter();
            reader.Open("test_data\\contacts.csv", CSVReaderWriter.Mode.Read);

            String name;
            String address;
            Assert.AreEqual(true, reader.Read("",""));
            Assert.AreEqual(true, reader.Read(out name, out address));
            Assert.AreEqual("Porter Coffey", name);
            Assert.AreEqual("Ap #827-9064 Sapien. Rd.|Palo Alto|Fl.|HM0G 0YR|Scotland", address);
        }

        [Test]
        public void test_read_from_csv_until_end()
        {
            var reader = new CSVReaderWriter();
            reader.Open("test_data\\contacts.csv", CSVReaderWriter.Mode.Read);

            int count = 0;
            while (reader.Read("", ""))
            {
                count++;
            }
            Assert.AreEqual(229, count);
        }

        [Test]
        public void test_write_to_csv()
        {
            var writer = new CSVReaderWriter();
            writer.Open("bogus.csv", CSVReaderWriter.Mode.Write);
            writer.Write(new [] {"John", "London"});
            writer.Write(new [] {"Doe", "nowhere" });
            writer.Close();

            writer.Open("bogus.csv", CSVReaderWriter.Mode.Read);

            String name;
            String address;
            Assert.AreEqual(true, writer.Read(out name, out address));
            Assert.AreEqual("John", name);
            Assert.AreEqual("London", address);
            Assert.AreEqual(true, writer.Read(out name, out address));
            Assert.AreEqual("Doe", name);
            Assert.AreEqual("nowhere", address);
            Assert.AreEqual(false, writer.Read(out name, out address));


        }

        [Test]
        public void test_closing_multiple_times_does_not_blow_up()
        {
            var reader = new CSVReaderWriter();
            reader.Open("test_data\\contacts.csv", CSVReaderWriter.Mode.Read);
            reader.Open("trololo.csv", CSVReaderWriter.Mode.Write);

            reader.Close();;
            reader.Close();;
        }
    }
}
