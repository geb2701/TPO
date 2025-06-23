using System.ComponentModel;

namespace Tpo.Domain
{
    public enum NivelHabilidad
    {
        [Description("Principiante (1): Sin experiencia o conocimientos b�sicos.")]
        Principiante = 1,

        [Description("B�sico (2): Conocimientos elementales y primeras pr�cticas.")]
        Basico = 2,

        [Description("Intermedio (3): Buen dominio de fundamentos y algo de experiencia.")]
        Intermedio = 3,

        [Description("Avanzado (4): Alto nivel de habilidad y experiencia considerable.")]
        Avanzado = 4,

        [Description("Experto (5): Dominio total y experiencia sobresaliente.")]
        Experto = 5
    }
}