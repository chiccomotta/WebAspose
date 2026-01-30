namespace WebAspose;

// Classi di esempio
public class ClassA
{
    [ToCompare]
    public string Property1 { get; set; }
    [ToCompare]
    public int? Property2 { get; set; }
    public DateTime? Property3 { get; set; } // Non comparata
}
    
public class ClassB
{
    public string Property1 { get; set; }
    public int? Property2 { get; set; }
}