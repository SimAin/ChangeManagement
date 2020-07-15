using System.Collections.Generic;

interface IDatabaseService<T> {
    IEnumerable<T> SelectAll ();
    void Insert (T obj);
}