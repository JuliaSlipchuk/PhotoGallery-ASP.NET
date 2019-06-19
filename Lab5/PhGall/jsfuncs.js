function hide_all()
        {
            var x = document.getElementsByClassName("DynamicButtons");
            var i;
            for (i = 0; i < x.length; i++) {
                x[i].style.visibility = 'hidden';
                //x[i].style.display ='none';
            }
        }