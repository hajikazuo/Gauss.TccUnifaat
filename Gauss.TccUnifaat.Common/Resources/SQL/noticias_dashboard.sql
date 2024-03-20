SELECT Count(*) as qtd, TipoNoticia
FROM Noticias
WHERE Excluido = 0
Group by TipoNoticia
Order by qtd desc