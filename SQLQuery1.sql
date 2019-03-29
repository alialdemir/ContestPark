select 
 sc.SubCategoryId, 
sc.Price,
 (case (SELECT
    (CASE
        WHEN EXISTS(
            SELECT NULL AS [EMPTY]
            FROM OpenSubCategories AS osc  where osc.UserId ='6e5e975a-6d12-414c-961d-edebdd7b4f0a' and osc.SubCategoryId = sc.SubCategoryId
            ) THEN 1
        ELSE 0
     END) )
  when 1 then 0 
  else sc.Price
end) as DisplayName,
 (case 
  when sc.Price = 0 then sc.PictuePath
  when (SELECT
    (CASE
        WHEN EXISTS(
            SELECT NULL AS [EMPTY]
            FROM OpenSubCategories AS osc  where osc.UserId ='6e5e975a-6d12-414c-961d-edebdd7b4f0a' and osc.SubCategoryId = sc.SubCategoryId
            ) THEN 1
        ELSE 0
     END) ) = 1 then sc.PictuePath
	 else 'assets/images/lock.png'
end) as PicturePath,
scl.SubCategoryName from SubCategories sc
INNER JOIN SubCategoryLangs scl on scl.SubCategoryId = sc.SubCategoryId and scl.LanguageId = 1
where sc.Visibility=1 and sc.ContestCategoryId =1
order by DisplayName