#!/bin/bash

# Colores
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # Sin color

# Emojis
CHECK_MARK="\xE2\x9C\x85"
CROSS_MARK="\xE2\x9D\x8C"
INFO="\xE2\x84\xB9"

# Función para mostrar la ayuda
mostrar_ayuda() {
    echo -e "${BLUE}Uso: $0 <directorio> <palabra_a_reemplazar> <palabra_nueva> [-c]${NC}"
    echo
    echo -e "${INFO} ${YELLOW}Este script reemplaza una palabra en los nombres de archivos y directorios dentro de un directorio y sus subdirectorios.${NC}"
    echo -e "${INFO} ${YELLOW}Opcionalmente, también puede reemplazar la palabra en el contenido de los archivos.${NC}"
    echo
    echo -e "${GREEN}Parámetros:${NC}"
    echo -e "  ${GREEN}<directorio>${NC}           ${YELLOW}El directorio donde buscar los archivos y directorios.${NC}"
    echo -e "  ${GREEN}<palabra_a_reemplazar>${NC} ${YELLOW}La palabra que deseas reemplazar en los nombres de archivos y directorios.${NC}"
    echo -e "  ${GREEN}<palabra_nueva>${NC}        ${YELLOW}La nueva palabra que reemplazará a la anterior.${NC}"
    echo
    echo -e "${GREEN}Opciones:${NC}"
    echo -e "  ${GREEN}-c${NC}                     ${YELLOW}Reemplaza la palabra también en el contenido de los archivos.${NC}"
    echo -e "  ${GREEN}-h, --help${NC}             ${YELLOW}Muestra esta ayuda y termina.${NC}"
}

# Verificar si se ha solicitado la ayuda
if [ "$#" -lt 3 ] || [ "$1" == "-h" ] || [ "$1" == "--help" ]; then
    mostrar_ayuda
    exit 1
fi

# Asignar los parámetros a variables
DIRECTORIO="$1"
PALABRA_A_REEMPLAZAR="$2"
PALABRA_NUEVA="$3"
REEMPLAZAR_CONTENIDO=false

# Verificar si se ha pasado la opción -c
if [ "$#" -eq 4 ] && [ "$4" == "-c" ]; then
    REEMPLAZAR_CONTENIDO=true
fi

# Encuentra y elimina todas las carpetas 'bin' y 'obj'
find "$DIRECTORIO" -type d \( -name 'bin' -o -name 'obj' \) -exec rm -rf {} +

# Si se ha solicitado, buscar y reemplazar en el contenido de los archivos
if [ "$REEMPLAZAR_CONTENIDO" = true ]; then
    find "$DIRECTORIO" -type f ! -name "*.DS_Store" | while read -r archivo; do
        if file "$archivo" | grep -q text; then
            echo -e "${INFO} ${YELLOW}Reemplazando en contenido del archivo:${NC} $archivo"
            if sed -i "s/$PALABRA_A_REEMPLAZAR/$PALABRA_NUEVA/g" "$archivo"; then
                echo -e "${CHECK_MARK} ${GREEN}Reemplazado en contenido:${NC} $archivo"
            else
                echo -e "${CROSS_MARK} ${RED}Error al reemplazar en contenido:${NC} $archivo"
            fi
        else
            echo -e "${INFO} ${YELLOW}Archivo no es de texto, se omite:${NC} $archivo"
        fi
    done
fi

# Buscar y reemplazar en los nombres de archivos
find "$DIRECTORIO" -type f -name "*$PALABRA_A_REEMPLAZAR*" | while read -r archivo; do
    nombre_archivo=$(basename "$archivo")
    nuevo_nombre=$(echo "$nombre_archivo" | sed "s/$PALABRA_A_REEMPLAZAR/$PALABRA_NUEVA/g")
    directorio=$(dirname "$archivo")
    nuevo_path="$directorio/$nuevo_nombre"
    echo -e "${INFO} ${YELLOW}Intentando renombrar archivo:${NC} $archivo ${YELLOW}a${NC} $nuevo_path"
    if mv "$archivo" "$nuevo_path"; then
        echo -e "${CHECK_MARK} ${GREEN}Archivo renombrado:${NC} $archivo ${GREEN}a${NC} $nuevo_path"
    else
        echo -e "${CROSS_MARK} ${RED}Error al renombrar archivo:${NC} $archivo"
    fi
done

# Buscar y reemplazar en los nombres de directorios
find "$DIRECTORIO" -depth -type d -name "*$PALABRA_A_REEMPLAZAR*" | while read -r dir; do
    nombre_directorio=$(basename "$dir")
    nuevo_nombre=$(echo "$nombre_directorio" | sed "s/$PALABRA_A_REEMPLAZAR/$PALABRA_NUEVA/g")
    directorio_padre=$(dirname "$dir")
    nuevo_path="$directorio_padre/$nuevo_nombre"
    echo -e "${INFO} ${YELLOW}Intentando renombrar directorio:${NC} $dir ${YELLOW}a${NC} $nuevo_path"
    if mv "$dir" "$nuevo_path"; then
        echo -e "${CHECK_MARK} ${GREEN}Directorio renombrado:${NC} $dir ${GREEN}a${NC} $nuevo_path"
    else
        echo -e "${CROSS_MARK} ${RED}Error al renombrar directorio:${NC} $dir"
    fi
done
