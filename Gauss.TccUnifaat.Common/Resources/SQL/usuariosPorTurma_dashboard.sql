SELECT 
    t.Nome as Turma, 
    COUNT(u.Id) as QuantidadeUsuariosPorTurma
FROM 
    Turmas t
    LEFT JOIN AspNetUsers u ON t.TurmaId = u.TurmaId AND (u.Excluido = 0 OR u.Excluido IS NULL)
WHERE 
    t.Excluido = 0
GROUP BY 
    t.Nome
ORDER BY 
    t.Nome;
