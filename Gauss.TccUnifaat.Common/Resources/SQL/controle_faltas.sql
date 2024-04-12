SELECT u.NomeCompleto, t.Nome AS NomeTurma, COUNT(*) AS QtdeFaltas
FROM Presencas p
INNER JOIN AspNetUsers u ON p.UsuarioId = u.Id
INNER JOIN Turmas t ON p.TurmaId = t.TurmaId
WHERE p.Presente = 0
GROUP BY u.NomeCompleto, p.UsuarioId, t.Nome
ORDER BY QtdeFaltas DESC;
