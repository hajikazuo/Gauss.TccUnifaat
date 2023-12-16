SELECT Count(*) as qtd, TipoNoticia
FROM Noticias
Group by TipoNoticia
Order by qtd desc