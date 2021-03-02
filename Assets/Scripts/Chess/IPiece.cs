using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Chess
{
    public interface IPiece
    {
        List<Box> CheckPossibleMovements();
        void Move(Box destination);



    }
}
