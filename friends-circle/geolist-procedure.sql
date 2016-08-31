CREATE PROCEDURE geodist @p_lat float, @p_lng float, @p_dist float
 
as

-- calculate lon and lat for the rectangle:
declare @lon1 float;
declare @lon2 float;
declare @lat1 float;
declare @lat2 float;

set @lon1 = @p_lng - @p_dist/abs(cos(radians(@p_lat))*69);
set @lon2 = @p_lng + @p_dist/abs(cos(radians(@p_lat))*69);
set @lat1 = @p_lat - (@p_dist/69); 
set @lat2 = @p_lat + (@p_dist/69);

SELECT *
FROM (
	select friends.*,3956 * 2 * ASIN(SQRT( POWER(SIN((@p_lat -friends.latitude) * pi()/180 / 2), 2) +COS(@p_lat * pi()/180) * COS(friends.latitude * pi()/180) *POWER(SIN((@p_lng -friends.longitude) * pi()/180 / 2), 2) )) as distance 
	from friends
	) f
WHERE longitude between @lon1 and @lon2 
	and latitude between @lat1 and @lat2 
	and Distance < @p_dist 
ORDER BY Distance;

go
