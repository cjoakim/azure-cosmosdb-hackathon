package com.microsoft.csu.cdbhack.rx;

import com.microsoft.csu.cdbhack.EnvVarNames;
import com.microsoft.csu.cdbhack.io.FileUtil;
import com.microsoft.csu.cdbhack.utils.CommandLineArgs;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import rx.Observable;

/**
 *
 * @author Chris Joakim, Microsoft
 * @date   2019/04/20
 */

@SuppressWarnings("MagicConstant")
public class RxPlay implements EnvVarNames {

    // Class variables
    private static final Logger logger = LoggerFactory.getLogger(RxPlay.class);
    private static CommandLineArgs clArgs = null;
    private static String function = null;
    
    // Instance variables

	public RxPlay() {
		// TODO Auto-generated constructor stub
	}

	public static void main(String[] args) throws Exception {
		
        clArgs = new CommandLineArgs(args);
        logger.warn("Main.main; args count: " + clArgs.count());
        function = clArgs.stringArg("--function", "none");
        logger.warn("Main.main; function: " + function);
        
        FileUtil fileUtil = new FileUtil();
        
        RxPlay rp = new RxPlay();
        Observable<String> observableLines = fileUtil.createFileLinesObservable("pom.xml");
        String infile = "pom.xml";

        // TODO - revisit this 
        //linesObservable.subscribe(rp::processLine);
	}
	
	private void processLine(String line) {
		
		System.out.println("processLine: " + line);
	}
}
