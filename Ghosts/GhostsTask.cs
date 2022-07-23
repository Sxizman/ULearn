using System;
using System.Text;

namespace hashes
{
	public class GhostsTask : 
		IFactory<Document>, IFactory<Vector>, IFactory<Segment>, IFactory<Cat>, IFactory<Robot>, 
		IMagic
	{
		private byte[] documentContent;
		private Document document;
		private Vector vector;
		private Segment segment;
		private Cat cat;
		private Robot robot;

		public GhostsTask()
        {
			documentContent = Encoding.UTF8.GetBytes("Example text");
			document = new Document("Doc", Encoding.UTF8, documentContent);
			vector = new Vector(2, 3);
			segment = new Segment(vector, new Vector(4, 2));
			cat = new Cat("Luna", "British Shorthair", new DateTime(2015, 6, 10));
			robot = new Robot("robot123", 95.2);
		}

		public void DoMagic()
		{
			documentContent[0] = 0;
			vector.Add(new Vector(1, 1));
			cat.Rename("Bella");
			Robot.BatteryCapacity++;
		}

		// Чтобы класс одновременно реализовывал интерфейсы IFactory<A> и IFactory<B> 
		// придется воспользоваться так называемой явной реализацией интерфейса.
		// Чтобы отличать методы создания A и B у каждого метода Create нужно явно указать, к какому интерфейсу он относится.
		// На самом деле такое вы уже видели, когда реализовывали IEnumerable<T>.

		Document IFactory<Document>.Create()
        {
			return document;
        }

		Vector IFactory<Vector>.Create()
		{
			return vector;
		}

		Segment IFactory<Segment>.Create()
		{
			return segment;
		}

		Cat IFactory<Cat>.Create()
        {
			return cat;
        }

		Robot IFactory<Robot>.Create()
        {
			return robot;
        }
	}
}