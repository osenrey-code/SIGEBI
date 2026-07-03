using SIGEBI.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIGEBI.Domain.Common
{
    public class Guard
    {
        public static void NotNull<TValue>(
            TValue value,
            string fieldName)
        {
            if (value is null)
            {
                throw new BusinessException(
                    $"{fieldName} es requerido."
                );
            }
        }

        public static void NotNullOrWhiteSpace(
            string? value,
            string fieldName,
            int? maxLen = null)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new BusinessException(
                    $"{fieldName} es requerido."
                );
            }

            if (maxLen.HasValue && value.Length > maxLen.Value)
            {
                throw new BusinessException(
                    $"{fieldName} no puede exceder {maxLen.Value} caracteres."
                );
            }
        }

        public static void GreaterThan(
            int value,
            int minExclusive,
            string fieldName)
        {
            if (value <= minExclusive)
            {
                throw new BusinessException(
                    $"{fieldName} debe ser mayor que {minExclusive}."
                );
            }
        }

        public static void GreaterThanD(
            decimal value,
            int minExclusive,
            string fieldName)
        {
            if (value <= minExclusive)
            {
                throw new BusinessException(
                    $"{fieldName} debe ser mayor que {minExclusive}."
                );
            }
        }

        public static void GreaterOrEqual(
            int value,
            int minInclusive,
            string fieldName)
        {
            if (value < minInclusive)
            {
                throw new BusinessException(
                    $"{fieldName} debe ser mayor o igual que {minInclusive}."
                );
            }
        }

        public static void GreaterOrEqual(
            decimal value,
            decimal minInclusive,
            string fieldName)
        {
            if (value < minInclusive)
            {
                throw new BusinessException(
                    $"{fieldName} debe ser mayor o igual que {minInclusive}."
                );
            }
        }

        public static void NotDefault(
            Guid value,
            string fieldName)
        {
            if (value == Guid.Empty)
            {
                throw new BusinessException(
                    $"{fieldName} es requerido."
                );
            }
        }
    }
}
