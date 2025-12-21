SET QUOTED_IDENTIFIER ON;
USE PDPW_DB;
GO

SET IDENTITY_INSERT Empresas ON;

INSERT INTO Empresas (Id, Nome, CNPJ, DataCriacao, Ativo) VALUES
(101, 'Empresa de Energia do Amazonas', '02341467000120', GETDATE(), 1),
(102, 'Companhia Energética de Pernambuco', '10835932000108', GETDATE(), 1),
(103, 'Companhia de Eletricidade da Bahia', '15139629000194', GETDATE(), 1),
(104, 'Companhia Energética do RN', '08324196000181', GETDATE(), 1),
(105, 'CPFL Paulista', '02429144000193', GETDATE(), 1),
(106, 'Enel SP', '61695227000193', GETDATE(), 1),
(107, 'Light', '60444437000171', GETDATE(), 1),
(108, 'Companhia Energética de Goiás', '01543032000165', GETDATE(), 1),
(109, 'Centrais Elétricas de SC', '83878892000156', GETDATE(), 1),
(110, 'Enel CE', '07047251000170', GETDATE(), 1),
(111, 'Energisa MS', '02016440000162', GETDATE(), 1),
(112, 'Energisa SE', '13092313000105', GETDATE(), 1),
(113, 'EDP ES', '28152650000174', GETDATE(), 1),
(114, 'Rio Grande Energia', '04895331000109', GETDATE(), 1),
(115, 'Enel RJ', '33050196000188', GETDATE(), 1),
(116, 'EDP SP', '61695731000193', GETDATE(), 1),
(117, 'Companhia Energética do Maranhão', '06272793000184', GETDATE(), 1);

SET IDENTITY_INSERT Empresas OFF;
GO

SELECT 'Empresas Adicionadas' as Status, COUNT(*) as Total FROM Empresas WHERE Id >= 101;
GO
