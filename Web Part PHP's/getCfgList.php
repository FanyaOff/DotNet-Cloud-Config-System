<?php
$dir = './cfgs';
 
$f = scandir($dir);
 
foreach ($f as $file){
    if(preg_match('/\.(txt)/', $file)){ // изменить под формат вашего кфг (default: txt)
        echo $file.'<br/>';
    }
}
?>