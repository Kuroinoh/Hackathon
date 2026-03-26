CREATE OR ALTER PROCEDURE sp_Usuario_Editar
    @idUsuario INT,
    @nombre NVARCHAR(70),
    @nombreUsuario NVARCHAR(30),
    @correo NVARCHAR(100),
    @idRol INT,
    @estado BIT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        UPDATE tbUsuario
        SET 
            nombre = @nombre,
            nombreUsuario = @nombreUsuario,
            correo = @correo,
            idRol = @idRol,
            estado = @estado
        WHERE idUsuario = @idUsuario;

        SELECT 1 AS resultado;
    END TRY
    BEGIN CATCH
        SELECT -1 AS resultado;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE sp_Usuario_Eliminar
    @idUsuario INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE tbUsuario
    SET estado = 0
    WHERE idUsuario = @idUsuario;

    SELECT 1 AS resultado;
END
GO
